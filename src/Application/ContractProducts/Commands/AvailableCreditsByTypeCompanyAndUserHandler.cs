using Application.ContractProducts.DTO;
using Application.Contracts.DTO;
using Application.Contracts.Queries;
using Application.Core;
using Domain.DTO;
using Domain.DTO.Products;
using Domain.Enums;
using Domain.Repositories;
using MediatR;

namespace Application.ContractProducts.Commands
{
    public class AvailableCreditsByTypeCompanyAndUserHandler : IRequestHandler<AvailableCreditsByTypeCompanyAndUserRequest, Result<CreditsAvailableByTypeCompanyAndUser>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IJobOfferRepository _jobOfferRepo;
        private readonly IEnterpriseUserJobVacRepository _enterpriseUserJobVacRepository;
        private readonly IMediator _mediatr;

        public AvailableCreditsByTypeCompanyAndUserHandler(IContractRepository contractRepository,
            IJobOfferRepository jobOfferRepository,
            IEnterpriseUserJobVacRepository enterpriseUserJobVacRepository, IMediator mediator)
        {
            _contractRepository = contractRepository;
            _jobOfferRepo = jobOfferRepository;
            _enterpriseUserJobVacRepository = enterpriseUserJobVacRepository;
            _mediatr = mediator;
        }

        private async Task<int> GetSoonestFinishingContractWithAvailableUnitsByType(List<ContractsDistDto> contracts, VacancyTypesCredits type, int ownerId)
        {
            List<AvailableUnitsDto> dto = new List<AvailableUnitsDto>();
            foreach (var c in contracts)
            {
                var data = await _mediatr.Send(new GetAvailableUnitsByOwner.Query
                {
                    ContractId = c.ContractId,
                    OwnerId = ownerId
                });
                if (data != null && data.Value.Count > 0 && data.Value.Sum(a => a.Units) > 0)
                {
                    var d = data.Value.Where(c => (int)c.type == (int)type).ToList();
                    if (d != null && d.Count > 0)
                    {
                        var line = d.First(); //should only be one?
                        line.FinishDate = _contractRepository.GetFinishDateByContract(c.ContractId);
                        dto.Add(line);
                    }
                }
            }
            return dto?.OrderBy(a => a.FinishDate).FirstOrDefault()?.ContractId ?? 0;
        }

        public async Task<Result<CreditsAvailableByTypeCompanyAndUser>> Handle(AvailableCreditsByTypeCompanyAndUserRequest request, CancellationToken cancellationToken)
        {
            CreditsAvailableByTypeCompanyAndUser creditsAvailableByTypeCompanyAndUser = new CreditsAvailableByTypeCompanyAndUser();

            var con = await _contractRepository.GetValidContracts(request.IDEnterprise, 6, 7);
            var contracts = con.Distinct();
            var contractIdsList = con.Select(c => c.ContractId).ToList();
            var alreadyAssignedCredits = await _enterpriseUserJobVacRepository.GetCreditsAssignedFromValidContracts(contractIdsList, request.IDEnterpriseUser);
            var groupedByTypeCredits = alreadyAssignedCredits.GroupBy(c => c.IdjobVacType).ToList();

            foreach (var types in groupedByTypeCredits)
            {
                if (types.Key == (int)VacancyTypesCredits.Basic)
                {
                    var sumAssigned = types.Sum(c => c.JobVacUsed);
                    var consumedBasic = contracts
                    .Where(c => c.IdJobVacType == (int)VacancyTypesCredits.Basic)
                    .Sum(c => _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(c.ContractId, (int)VacancyTypesCredits.Basic)
                    .Count(a => a.IdenterpriseUserG == request.IDEnterpriseUser));
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 130,
                        ProductName = "Basic",
                        Credits = sumAssigned - consumedBasic,
                        ContractIdFinishingSoonest = await GetSoonestFinishingContractWithAvailableUnitsByType(contracts.ToList(), VacancyTypesCredits.Basic, request.IDEnterpriseUser),
                        InternationalDifusion = false,
                        NationalDifusion = false,
                        IsBlindable = false,
                        IsFeatured = false,
                        IdJobVacType = 0
                    };
                    if(productUnitsForPublishOffer.Credits >0 && productUnitsForPublishOffer.ContractIdFinishingSoonest> 0)
                    {
                        creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                    }
                    
                }
                if(types.Key == (int) VacancyTypesCredits.Superior)
                {
                    var sumAssigned = types.Sum(c => c.JobVacUsed);
                    var consumedSuperior = contracts
                        .Where(c => c.IdJobVacType == (int)VacancyTypesCredits.Superior)
                        .Sum(c => _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(c.ContractId, (int)VacancyTypesCredits.Superior)
                        .Count(a => a.IdenterpriseUserG == request.IDEnterpriseUser));
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 4,
                        ProductName = "Superior",
                        Credits = sumAssigned - consumedSuperior,
                        ContractIdFinishingSoonest = await GetSoonestFinishingContractWithAvailableUnitsByType(contracts.ToList(), VacancyTypesCredits.Superior, request.IDEnterpriseUser),
                        InternationalDifusion = false,
                        NationalDifusion = false,
                        IsBlindable = true,
                        IsFeatured = false,
                        IdJobVacType = 2,
                    };
                    if(productUnitsForPublishOffer.Credits >0 && productUnitsForPublishOffer.ContractIdFinishingSoonest> 0)
                    {
                        creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                    }
                }
                if(types.Key == (int) VacancyTypesCredits.Premium)
                {
                    var sumAssigned = types.Sum(c => c.JobVacUsed);
                    var consumedPremium = contracts
                        .Where(c => c.IdJobVacType == (int)VacancyTypesCredits.Premium)
                        .Sum(c => _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(c.ContractId, (int)VacancyTypesCredits.Premium)
                                               .Count(a => a.IdenterpriseUserG == request.IDEnterpriseUser));
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 87,
                        ProductName = "Premium",
                        Credits = sumAssigned - consumedPremium,
                        ContractIdFinishingSoonest = await GetSoonestFinishingContractWithAvailableUnitsByType(contracts.ToList(), VacancyTypesCredits.Premium, request.IDEnterpriseUser),
                        InternationalDifusion = false,
                        NationalDifusion = true,
                        IsBlindable = true,
                        IsFeatured = true,
                        IdJobVacType = 1,
                    };
                    if(productUnitsForPublishOffer.Credits >0 && productUnitsForPublishOffer.ContractIdFinishingSoonest> 0)
                    {
                        creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                    }
                }
                if(types.Key == (int) VacancyTypesCredits.PremiumInternational)
                {
                    var sumAssigned = types.Sum(c => c.JobVacUsed);
                    var consumedPremiumInternational = contracts
                        .Where(c => c.IdJobVacType == (int)VacancyTypesCredits.PremiumInternational)
                        .Sum(c => _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(c.ContractId, (int)VacancyTypesCredits.PremiumInternational)
                                                                      .Count(a => a.IdenterpriseUserG == request.IDEnterpriseUser));
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 125,
                        ProductName = "Premium International",
                        Credits = sumAssigned - consumedPremiumInternational,
                        ContractIdFinishingSoonest = await GetSoonestFinishingContractWithAvailableUnitsByType(contracts.ToList(), VacancyTypesCredits.PremiumInternational, request.IDEnterpriseUser),
                        InternationalDifusion = true,
                        NationalDifusion = true,
                        IsBlindable = true,
                        IsFeatured = true,
                        IdJobVacType = 3,
                    };
                    if(productUnitsForPublishOffer.Credits >0 && productUnitsForPublishOffer.ContractIdFinishingSoonest> 0)
                    {
                        creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                    }
                }

                if(types.Key == (int) VacancyTypesCredits.Internship)
                {
                    var sumAssigned = types.Sum(c => c.JobVacUsed);
                    var consumedInternship = contracts
                        .Where(c => c.IdJobVacType == (int)VacancyTypesCredits.Internship)
                        .Sum(c => _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(c.ContractId, (int)VacancyTypesCredits.Internship)
                                                                      .Count(a => a.IdenterpriseUserG == request.IDEnterpriseUser));
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 264,
                        ProductName = "Internship",
                        Credits = sumAssigned - consumedInternship,
                        ContractIdFinishingSoonest = await GetSoonestFinishingContractWithAvailableUnitsByType(contracts.ToList(), VacancyTypesCredits.Internship, request.IDEnterpriseUser),
                        InternationalDifusion = false,
                        NationalDifusion = false,
                        IsBlindable = false,
                        IsFeatured = false,
                        IdJobVacType = 4
                    };
                    if(productUnitsForPublishOffer.Credits >0 && productUnitsForPublishOffer.ContractIdFinishingSoonest> 0)
                    {
                        creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                    }
                    
                }
                if(types.Key == (int) VacancyTypesCredits.StandardWelcome)
                {
                    var sumAssigned = types.Sum(c => c.JobVacUsed);
                    var consumedStandardWelcome = contracts
                        .Where(c => c.IdJobVacType == (int)VacancyTypesCredits.StandardWelcome)
                        .Sum(c => _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(c.ContractId, (int)VacancyTypesCredits.StandardWelcome)
                                                                                             .Count(a => a.IdenterpriseUserG == request.IDEnterpriseUser));
                    ProductUnitsForPublishOffer productUnitsForPublishOffer = new()
                    {
                        IsStandard = true,
                        ProductId = 110,
                        ProductName = "Basic welcome",
                        Credits = sumAssigned - consumedStandardWelcome,
                        ContractIdFinishingSoonest = await GetSoonestFinishingContractWithAvailableUnitsByType(contracts.ToList(), VacancyTypesCredits.StandardWelcome, request.IDEnterpriseUser),
                        InternationalDifusion = false,
                        NationalDifusion = false,
                        IsBlindable = true,
                        IsFeatured = false,
                        IdJobVacType = 6
                    };
                    if(productUnitsForPublishOffer.Credits >0 && productUnitsForPublishOffer.ContractIdFinishingSoonest> 0)
                    {
                        creditsAvailableByTypeCompanyAndUser.ProductCredits.Add(productUnitsForPublishOffer);
                    }
                }
            }

            return Result<CreditsAvailableByTypeCompanyAndUser>.Success(creditsAvailableByTypeCompanyAndUser);
        }
    }
}
