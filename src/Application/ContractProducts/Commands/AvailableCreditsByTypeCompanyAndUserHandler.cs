using Application.ContractProducts.DTO;
using Application.Core;
using Domain.DTO.Products;
using Domain.Enums;
using Domain.Repositories;
using MediatR;

namespace Application.ContractProducts.Commands
{
    public class AvailableCreditsByTypeCompanyAndUserHandler : IRequestHandler<AvailableCreditsByTypeCompanyAndUserRequest, Result<CreditsAvailableByTypeCompanyAndUser>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IRegEnterpriseContractRepository _regEnterpriseContract;
        private readonly IJobOfferRepository _jobOfferRepo;
        private readonly IUserRepository _userRepo;
        private readonly IEnterpriseUserJobVacRepository _enterpriseUserJobVacRepository;

        public AvailableCreditsByTypeCompanyAndUserHandler(IContractRepository contractRepository,
            IJobOfferRepository jobOfferRepository,
            IRegEnterpriseContractRepository regEnterpriseContractRepository,
            IUserRepository userRepo,
            IEnterpriseUserJobVacRepository enterpriseUserJobVacRepository)
        {
            _contractRepository = contractRepository;
            _jobOfferRepo = jobOfferRepository;
            _regEnterpriseContract = regEnterpriseContractRepository;
            _userRepo = userRepo;
            _enterpriseUserJobVacRepository = enterpriseUserJobVacRepository;
        }

        public async Task<Result<CreditsAvailableByTypeCompanyAndUser>> Handle(AvailableCreditsByTypeCompanyAndUserRequest request, CancellationToken cancellationToken)
        {
            CreditsAvailableByTypeCompanyAndUser creditsAvailableByTypeCompanyAndUser = new CreditsAvailableByTypeCompanyAndUser();

            var contracts = await _contractRepository.GetValidContracts(request.IDEnterprise, 6, 7);

            int[] typesInValidContracts = contracts.Select(a => a.IdJobVacType).Distinct().ToList().ToArray();
            var purchasedBasic = contracts.Sum(c => _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.Basic));
            var purchasedSuperior = contracts.Sum(c => _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.Superior));
            var purchasedPremium = contracts.Sum(c => _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.Premium));
            var purchasedPremiumInternational = contracts.Sum(c => _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.PremiumInternational));
            var purchasedInternship = contracts.Sum(c => _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.Internship));
            var standardWelcome = contracts.Sum(c => _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.StandardWelcome));


            var consumedBasic = contracts.Sum(c => _jobOfferRepo.GetActiveOffersByContractAndType(c.ContractId, 0).Where(a => a.IdenterpriseUserG == request.IDEnterpriseUser).Count());
            var consumedSuperior = contracts.Sum(c => _jobOfferRepo.GetActiveOffersByContractAndType(c.ContractId, 2).Where(a => a.IdenterpriseUserG == request.IDEnterpriseUser).Count());
            var consumedPremium = contracts.Sum(c => _jobOfferRepo.GetActiveOffersByContractAndType(c.ContractId, 1).Where(a => a.IdenterpriseUserG == request.IDEnterpriseUser).Count());
            var consumedPremiumInternational = contracts.Sum(c => _jobOfferRepo.GetActiveOffersByContractAndType(c.ContractId, 3).Where(a => a.IdenterpriseUserG == request.IDEnterpriseUser).Count());
            var consumedInternship = contracts.Sum(c => _jobOfferRepo.GetActiveOffersByContractAndType(c.ContractId, 4).Where(a => a.IdenterpriseUserG == request.IDEnterpriseUser).Count());
            var consumedStandardWelcome = contracts.Sum(c => _jobOfferRepo.GetActiveOffersByContractAndType(c.ContractId, 6).Where(a => a.IdenterpriseUserG == request.IDEnterpriseUser).Count());

            var user = _userRepo.GetCompanyValidUsers(request.IDEnterprise).FirstOrDefault(u => u.IdEnterpriseUser == request.IDEnterpriseUser);
            var assignedBasic = 0;
            var assignedSuperior = 0;
            var assignedPremium = 0;
            var assignedPremiumInternational = 0;
            var assignedInternship = 0;
            var assignedStandardWelcome = 0;

            foreach (var c in contracts)
            {
                var assBasic = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(request.IDEnterpriseUser, (int)VacancyTypesCredits.Basic, c.ContractId);
                assignedBasic += assBasic.Sum(b => b.JobVacUsed);
                var assSuperior = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(request.IDEnterpriseUser, (int)VacancyTypesCredits.Superior, c.ContractId);
                assignedSuperior += assSuperior.Sum(b => b.JobVacUsed);
                var assPremium = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(request.IDEnterpriseUser, (int)VacancyTypesCredits.Premium, c.ContractId);
                assignedPremium += assPremium.Sum(b => b.JobVacUsed);
                var assPremiumInternational = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(request.IDEnterpriseUser, (int)VacancyTypesCredits.PremiumInternational, c.ContractId);
                assignedPremiumInternational = assPremiumInternational.Sum(b => b.JobVacUsed);
                var assInternship = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(request.IDEnterpriseUser, (int)VacancyTypesCredits.Internship, c.ContractId);
                assignedInternship += assInternship.Sum(b => b.JobVacUsed);
                var assWelcome = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContract(request.IDEnterpriseUser, (int)VacancyTypesCredits.StandardWelcome, c.ContractId);
                assignedStandardWelcome += assWelcome.Sum(b => b.JobVacUsed);
            }

            var availableBasic = assignedBasic - consumedBasic ;
            var availableSuperior = assignedSuperior - consumedSuperior;
            var availablePremium = assignedPremium - consumedPremium;
            var availablePremiumInternational = assignedPremiumInternational - consumedPremiumInternational;
            var availableInternship = assignedInternship - consumedInternship;
            var availableStandardWelcome = assignedStandardWelcome - consumedStandardWelcome;
            //from precedent available credits get the older contract
            foreach (var type in typesInValidContracts)
            {
                if (type == (int)VacancyTypesCredits.Basic && availableBasic > 0)
                {
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 130,
                        ProductName = "Basic",
                        Credits = availableBasic,
                        OlderContractId = contracts.Where(c => c.IdJobVacType == type).OrderBy(c => c.StartDate).FirstOrDefault().ContractId,
                        InternationalDifusion = false,
                        NationalDifusion = false,
                        IsBlindable = false,
                        IdJobVacType = 0
                    };
                    creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                }
                else if(type == (int) VacancyTypesCredits.Superior && availableSuperior >0)
                {
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 4,
                        ProductName = "Superior",
                        Credits = availableSuperior,
                        OlderContractId = contracts.Where(c => c.IdJobVacType == type).OrderBy(c => c.StartDate).FirstOrDefault().ContractId,
                        InternationalDifusion = false,
                        NationalDifusion = false,
                        IsBlindable = true,
                        IdJobVacType = 2,

                    };
                    creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                }
                else if(type == (int) VacancyTypesCredits.Premium && availablePremium >0)
                {
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 87,
                        ProductName = "Premium",
                        Credits = availablePremium,
                        OlderContractId = contracts.Where(c => c.IdJobVacType == type).OrderBy(c => c.StartDate).FirstOrDefault().ContractId,
                        InternationalDifusion = false,
                        NationalDifusion = true,
                        IsBlindable = true,
                        IdJobVacType = 1,
                    };
                    creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                }
                else if(type == (int) VacancyTypesCredits.PremiumInternational && availablePremiumInternational >0)
                {
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 125,
                        ProductName = "Premium International",
                        Credits = availablePremiumInternational,
                        OlderContractId = contracts.Where(c => c.IdJobVacType == type).OrderBy(c => c.StartDate).FirstOrDefault().ContractId,
                        InternationalDifusion = true,
                        NationalDifusion = true,
                        IsBlindable = true,
                        IdJobVacType = 1,
                    };
                    creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                }
                else if(type == (int)VacancyTypesCredits.Internship && availableInternship >0)
                {
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 264,
                        ProductName = "Internship",
                        Credits = availableInternship,
                        OlderContractId = contracts.Where(c => c.IdJobVacType == type).OrderBy(c => c.StartDate).FirstOrDefault().ContractId,
                        InternationalDifusion = false,
                        NationalDifusion = false,
                        IsBlindable = false,
                        IdJobVacType = 4
                    };
                    creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                }
                else if(type == (int) VacancyTypesCredits.StandardWelcome && availableStandardWelcome > 0)
                {
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 110,
                        ProductName = "Basic welcome",
                        Credits = availableStandardWelcome,
                        OlderContractId = contracts.Where(c => c.IdJobVacType == type).OrderBy(c => c.StartDate).FirstOrDefault().ContractId,
                        InternationalDifusion = false,
                        NationalDifusion = false,
                        IsBlindable = false,
                        IdJobVacType = 6
                    };
                    creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                }
            }
            return Result<CreditsAvailableByTypeCompanyAndUser>.Success(creditsAvailableByTypeCompanyAndUser);
        }
    }
}
