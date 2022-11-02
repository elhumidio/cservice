using Application.Contracts.DTO;
using Application.Contracts.Queries;
using Application.DTO;
using Domain.Enums;
using Domain.Repositories;
using MediatR;

namespace Application.EnterpriseContract.Queries
{
    public class GetContract
    {
        public class Query : IRequest<ContractResult>
        {
            public int CompanyId { get; set; }
            public VacancyType? type { get; set; }
            public int RegionId { get; set; }
        }

        public class Handler : IRequestHandler<Query, ContractResult>
        {
            private readonly IMediator _mediatr;
            private readonly IContractPublicationRegionRepository _contractPublicationRegionRepository;

            public Handler(IMediator mediatr, IContractPublicationRegionRepository contractPublicationRegionRepository)
            {
                _mediatr = mediatr;
                _contractPublicationRegionRepository = contractPublicationRegionRepository;
            }

            public async Task<ContractResult> Handle(Query request, CancellationToken cancellationToken)
            {
                List<string> failures = new List<string>();
                var contractToUse = new ContractDto();
                var contractsRegionAllowed = new List<ContractDto>();
                var regions = new List<int>();
                var contracts = _mediatr.Send(new List.Query
                {
                    CompanyId = request.CompanyId,
                }).Result;
                if (!contracts.Value.Any())
                    failures.Add("No contracts available.\n\r");

                foreach (var contract in contracts.Value)
                {
                    regions = _contractPublicationRegionRepository.AllowedRegionsByContract(contract.Idcontract);
                    bool IsContractWithAllowedRegion = regions.Contains(request.RegionId) || !regions.Any();

                    if (IsContractWithAllowedRegion)
                    {
                        contractsRegionAllowed.Add(contract);
                    }
                    else failures.Add("Region not allowed, contact to Customer Service.\n\r");
                }

                if (contractsRegionAllowed.Any())
                {
                    foreach (var (contract, type) in from contract in contractsRegionAllowed
                                                     from type in contract.RegEnterpriseContracts
                                                     select (contract, type))
                    {
                        if (request.type == VacancyType.None)
                        {
                            var units = _mediatr
                                .Send(new GetAvailableUnits.Query { ContractId = contract.Idcontract })
                                .Result
                                .Value;
                            var unitsAvailable = units.Where(s => s.Units > 0);

                            if ((unitsAvailable.Sum(u => u.Units)) > 0)
                            {
                                contractToUse = contract;
                                var unitsToUse = unitsAvailable.FirstOrDefault(ua => ua.Units > 0);
                                if (unitsToUse != null)
                                {
                                    contractToUse.IdJobVacType = (int)unitsToUse.type;
                                }
                                break;
                            }
                            else failures.Add("No units available.\n\r");
                        }
                        else
                        {
                            var units = _mediatr
                                .Send(new GetAvailableUnits.Query { ContractId = contract.Idcontract })
                                .Result
                                .Value.Where(u => u.type == request.type).Sum(r => r.Units);

                            var canUse = (request.type == (VacancyType)type.IdjobVacType) && (units > 0);
                            if (canUse)
                            {
                                contractToUse = contract;
                                contractToUse.IdJobVacType = type.IdjobVacType;
                                break;
                            }
                            else failures.Add("No units available.\n\r");
                        }
                    }
                }
                if (contractToUse.Idcontract == 0)
                    return ContractResult.Failure(failures);
                else
                    return ContractResult.Success(await Task.FromResult(contractToUse));
            }
        }
    }
}
