using Application.Contracts.DTO;
using Application.Core;
using Domain.DTO;
using Domain.Enums;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class GetCountAvailableUnitsByCompaniesIds
    {
        public class Get : IRequest<Result<IReadOnlyList<KeyValueResponse>>>
        {
            public List<int> CompaniesIds { get; set; }
        }

        public class Handler : IRequestHandler<Get, Result<IReadOnlyList<KeyValueResponse>>>
        {
            private readonly IContractRepository _contractRepository;
            private readonly IJobOfferRepository _jobOfferRepo;
            private readonly IContractProductRepository _contractProductRepo;
            private readonly IUnitsRepository _unitsRepo;

            public Handler(IJobOfferRepository jobOfferRepo, IContractRepository contractRepository, IContractProductRepository contractProductRepo, IUnitsRepository unitsRepo)
            {
                _jobOfferRepo = jobOfferRepo;
                _contractProductRepo = contractProductRepo;
                _unitsRepo = unitsRepo;
                _contractRepository = contractRepository;
            }

            public async Task<Result<IReadOnlyList<KeyValueResponse>>> Handle(Get request, CancellationToken cancellationToken)
            {
                List<KeyValueResponse> keyValueResponse = new List<KeyValueResponse>();
                List<EnterpriseListContractsIdsDto> enterpriseContracts = (await _contractRepository.GetContractsByCompaniesIds(request.CompaniesIds)).ToList();

                foreach(var enterpriseContractData in enterpriseContracts)
                {
                    int key = enterpriseContractData.Id;
                    List<int> enterpriseContractIds = enterpriseContractData.Value;
                    int countUnitsPerEnterprise = 0;

                    foreach (int enterpriseContractId in enterpriseContractIds)
                    {
                        var list = new List<AvailableUnitsDto>();
                        AvailableUnitsDto dto;
                        var isPack = _contractProductRepo.IsPack(enterpriseContractId);
                        var unitsAssigned = _unitsRepo.GetAssignmentsByContract(enterpriseContractId).ToList();

                        foreach (var units in unitsAssigned)
                        {
                            var unitsConsumed = isPack ? _jobOfferRepo.GetActiveOffersByContractOwnerType(enterpriseContractId, units.IdenterpriseUser, units.IdjobVacType).Count()
                                 : _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(enterpriseContractId, units.IdjobVacType).Count();

                            countUnitsPerEnterprise = countUnitsPerEnterprise + (units.JobVacUsed - unitsConsumed);
                        }
                    }

                    KeyValueResponse itemEnterprise = new KeyValueResponse();
                    itemEnterprise.Id = key;
                    itemEnterprise.Value = countUnitsPerEnterprise;

                    keyValueResponse.Add(itemEnterprise);
                }

                return Result<IReadOnlyList<KeyValueResponse>>.Success(keyValueResponse);
            }
        }
    }
}
