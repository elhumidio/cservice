using Application.ContractProducts.DTO;
using Application.Contracts.DTO;
using Application.Contracts.Queries;
using Application.Core;
using Domain.DTO;
using Domain.DTO.Distribution;
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
        private readonly IMediator _mediatr;

        public AvailableCreditsByTypeCompanyAndUserHandler(IContractRepository contractRepository,
            IJobOfferRepository jobOfferRepository,
            IRegEnterpriseContractRepository regEnterpriseContractRepository,
            IUserRepository userRepo,
            IEnterpriseUserJobVacRepository enterpriseUserJobVacRepository, IMediator mediator)
        {
            _contractRepository = contractRepository;
            _jobOfferRepo = jobOfferRepository;
            _regEnterpriseContract = regEnterpriseContractRepository;
            _userRepo = userRepo;
            _enterpriseUserJobVacRepository = enterpriseUserJobVacRepository;
            _mediatr = mediator;
        }

        private async Task<int> GetOlderContractWithAvailableUnitsByType(List<ContractsDistDto> contracts, VacancyTypesCredits type, int ownerId)
        {
            List<AvailableUnitsDto> dto = new List<AvailableUnitsDto>();
            foreach (var c in contracts)
            {
                var data = await _mediatr.Send(new GetAvailableUnitsByOwner.Query
                {
                     ContractId = c.ContractId,
                      OwnerId = ownerId
                });
                if(data != null && data.Value.Count() > 0 && data.Value.FirstOrDefault().Units > 0)
                {
                    var d = data.Value.Where(c => (int)c.type == (int)type).ToList();
                    if(d != null && d.Count() > 0)
                    {
                        d.FirstOrDefault().StartDate = _contractRepository.GetStartDateByContract(c.ContractId);
                        dto.Add(d.FirstOrDefault());
                    }
    
                    
                }

            }
            return dto?.OrderByDescending(a => a.StartDate).FirstOrDefault()?.ContractId ?? 0;

        }

        public async Task<Result<CreditsAvailableByTypeCompanyAndUser>> Handle(AvailableCreditsByTypeCompanyAndUserRequest request, CancellationToken cancellationToken)
        {
            CreditsAvailableByTypeCompanyAndUser creditsAvailableByTypeCompanyAndUser = new CreditsAvailableByTypeCompanyAndUser();

            var con = await _contractRepository.GetValidContracts(request.IDEnterprise, 6, 7);
            var contracts = con.Distinct();

            int[] typesInValidContracts = contracts.Select(a => a.IdJobVacType).Distinct().ToList().ToArray();
            var purchasedBasic = contracts.Sum(c => c.IdJobVacType == (int)VacancyTypesCredits.Basic
                                                    ? _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.Basic)
                                                    : 0);

            var purchasedSuperior = contracts.Sum(c => c.IdJobVacType == (int)VacancyTypesCredits.Superior
                                        ? _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.Superior)
                                        : 0);

            var purchasedPremium = contracts.Sum(c => c.IdJobVacType == (int)VacancyTypesCredits.Premium
                            ? _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.Premium)
                            : 0);
            var purchasedPremiumInternational = contracts.Sum(c => c.IdJobVacType == (int)VacancyTypesCredits.PremiumInternational
                ? _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.PremiumInternational)
                : 0);

            var purchasedInternship = contracts.Sum(c => c.IdJobVacType == (int)VacancyTypesCredits.Internship
                ? _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.Internship)
                : 0);

            var standardWelcome = contracts.Sum(c => c.IdJobVacType == (int)VacancyTypesCredits.StandardWelcome
                            ? _regEnterpriseContract.GetUnitsByCreditType(c.ContractId, VacancyTypesCredits.StandardWelcome)
                            : 0);

            var consumedBasic = contracts
              .Where(c => c.IdJobVacType == (int)VacancyTypesCredits.Basic)
              .Sum(c => _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(c.ContractId, (int)VacancyTypesCredits.Basic)
                  .Count(a => a.IdenterpriseUserG == request.IDEnterpriseUser));
            var consumedSuperior = contracts
              .Where(c => c.IdJobVacType == (int)VacancyTypesCredits.Superior)
              .Sum(c => _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(c.ContractId, (int)VacancyTypesCredits.Superior)
                  .Count(a => a.IdenterpriseUserG == request.IDEnterpriseUser));
            var consumedPremium = contracts
              .Where(c => c.IdJobVacType == (int)VacancyTypesCredits.Premium)
              .Sum(c => _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(c.ContractId, (int)VacancyTypesCredits.Premium)
                  .Count(a => a.IdenterpriseUserG == request.IDEnterpriseUser));

            var consumedPremiumInternational = contracts
              .Where(c => c.IdJobVacType == (int)VacancyTypesCredits.PremiumInternational)
              .Sum(c => _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(c.ContractId, (int)VacancyTypesCredits.PremiumInternational)
                  .Count(a => a.IdenterpriseUserG == request.IDEnterpriseUser));

            var consumedInternship = contracts
              .Where(c => c.IdJobVacType == (int)VacancyTypesCredits.Internship)
              .Sum(c => _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(c.ContractId, (int)VacancyTypesCredits.Internship)
                  .Count(a => a.IdenterpriseUserG == request.IDEnterpriseUser));

            var consumedStandardWelcome = contracts
              .Where(c => c.IdJobVacType == (int)VacancyTypesCredits.StandardWelcome)
              .Sum(c => _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(c.ContractId, (int)VacancyTypesCredits.StandardWelcome)
                  .Count(a => a.IdenterpriseUserG == request.IDEnterpriseUser));


            var user = _userRepo.GetCompanyValidUsers(request.IDEnterprise).FirstOrDefault(u => u.IdEnterpriseUser == request.IDEnterpriseUser);
            var assignedBasic = 0;
            var assignedSuperior = 0;
            var assignedPremium = 0;
            var assignedPremiumInternational = 0;
            var assignedInternship = 0;
            var assignedStandardWelcome = 0;
            var ListBassicAssignments = new List<EnterpriseUserJobVacDto>();
            var ListSuperiorAssignments = new List<EnterpriseUserJobVacDto>();
            var ListPremiumAssignments = new List<EnterpriseUserJobVacDto>();
            var ListPremiumInternacionalAssignemts = new List<EnterpriseUserJobVacDto>();
            var ListInternShipAssignments = new List<EnterpriseUserJobVacDto>();
            var ListStandardWelcomeAssignments = new List<EnterpriseUserJobVacDto>();

            foreach (var c in contracts)
            {
                var assBasic = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContractForOffers(request.IDEnterpriseUser, (int)VacancyTypesCredits.Basic, c.ContractId);
                ListBassicAssignments.AddRange(assBasic);
                assignedBasic += assBasic.Sum(b => b.JobVacUsed);
                var assSuperior = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContractForOffers(request.IDEnterpriseUser, (int)VacancyTypesCredits.Superior, c.ContractId);
                ListSuperiorAssignments.AddRange(assSuperior);
                assignedSuperior += assSuperior.Sum(b => b.JobVacUsed);
                var assPremium = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContractForOffers(request.IDEnterpriseUser, (int)VacancyTypesCredits.Premium, c.ContractId);
                ListPremiumAssignments.AddRange(assPremium);
                assignedPremium += assPremium.Sum(b => b.JobVacUsed);
                var assPremiumInternational = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContractForOffers(request.IDEnterpriseUser, (int)VacancyTypesCredits.PremiumInternational, c.ContractId);
                ListPremiumInternacionalAssignemts.AddRange(assPremiumInternational);
                assignedPremiumInternational = assPremiumInternational.Sum(b => b.JobVacUsed);
                var assInternship = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContractForOffers(request.IDEnterpriseUser, (int)VacancyTypesCredits.Internship, c.ContractId);
                ListInternShipAssignments.AddRange(assInternship);
                assignedInternship += assInternship.Sum(b => b.JobVacUsed);
                var assWelcome = await _enterpriseUserJobVacRepository.GetAssignmentsByUserProductAndContractForOffers(request.IDEnterpriseUser, (int)VacancyTypesCredits.StandardWelcome, c.ContractId);
                ListStandardWelcomeAssignments.AddRange(assWelcome);
                assignedStandardWelcome += assWelcome.Sum(b => b.JobVacUsed);
            }

            var availableBasic = assignedBasic - consumedBasic;
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
                    var contr = await _contractRepository.GetValidContracts(request.IDEnterprise, 6, 7);

                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 130,
                        ProductName = "Basic",
                        Credits = availableBasic,
                        OlderContractId = await GetOlderContractWithAvailableUnitsByType(contracts.ToList(), VacancyTypesCredits.Basic, request.IDEnterpriseUser),
                        InternationalDifusion = false,
                        NationalDifusion = false,
                        IsBlindable = false,
                        IdJobVacType = 0
                    };
                    creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                }
                else if (type == (int)VacancyTypesCredits.Superior && availableSuperior > 0)
                {
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 4,
                        ProductName = "Superior",
                        Credits = availableSuperior,                     
                        OlderContractId = await GetOlderContractWithAvailableUnitsByType(contracts.ToList(), VacancyTypesCredits.Superior, request.IDEnterpriseUser),
                        InternationalDifusion = false,
                        NationalDifusion = false,
                        IsBlindable = true,
                        IdJobVacType = 2,
                    };
                    creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                }
                else if (type == (int)VacancyTypesCredits.Premium && availablePremium > 0)
                {
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 87,
                        ProductName = "Premium",
                        Credits = availablePremium,            
                        OlderContractId = await GetOlderContractWithAvailableUnitsByType(contracts.ToList(), VacancyTypesCredits.Premium, request.IDEnterpriseUser),
                        InternationalDifusion = false,
                        NationalDifusion = true,
                        IsBlindable = true,
                        IdJobVacType = 1,
                    };
                    creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                }
                else if (type == (int)VacancyTypesCredits.PremiumInternational && availablePremiumInternational > 0)
                {
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 125,
                        ProductName = "Premium International",
                        Credits = availablePremiumInternational,    
                        OlderContractId = await GetOlderContractWithAvailableUnitsByType(contracts.ToList(), VacancyTypesCredits.PremiumInternational, request.IDEnterpriseUser),
                        InternationalDifusion = true,
                        NationalDifusion = true,
                        IsBlindable = true,
                        IdJobVacType = 1,
                    };
                    creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                }
                else if (type == (int)VacancyTypesCredits.Internship && availableInternship > 0)
                {
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 264,
                        ProductName = "Internship",
                        Credits = availableInternship,
                        OlderContractId = await GetOlderContractWithAvailableUnitsByType(contracts.ToList(), VacancyTypesCredits.Internship, request.IDEnterpriseUser),
                        InternationalDifusion = false,
                        NationalDifusion = false,
                        IsBlindable = false,
                        IdJobVacType = 4
                    };
                    creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                }
                else if (type == (int)VacancyTypesCredits.StandardWelcome && availableStandardWelcome > 0)
                {
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 110,
                        ProductName = "Basic welcome",
                        Credits = availableStandardWelcome,
                        OlderContractId = await GetOlderContractWithAvailableUnitsByType(contracts.ToList(), VacancyTypesCredits.StandardWelcome, request.IDEnterpriseUser),
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
