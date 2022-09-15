using Application.Contracts.DTO;
using Application.Contracts.Queries;
using Application.DTO;
using AutoMapper;
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
            private IMediator _mediatr;
            private readonly IContractRepository _contractRepository;
            private readonly IMapper _mapper;
            private readonly IContractPublicationRegionRepository _contractPublicationRegionRepository;

            public Handler(IMapper mapper, IContractRepository contractRepository, IMediator mediatr, IContractPublicationRegionRepository contractPublicationRegionRepository)
            {
                _mapper = mapper;
                _contractRepository = contractRepository;
                _mediatr = mediatr;
                _contractPublicationRegionRepository = contractPublicationRegionRepository;
            }

            public async Task<ContractResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var contractToUse = new ContractDto();
                var contractsRegionAllowed = new List<ContractDto>();
                var regions = new List<int>();
                var contracts = _mediatr.Send(new List.Query
                {
                    CompanyId = request.CompanyId,
                }).Result;

                foreach (var contract in contracts.Value)
                {
                    regions = _contractPublicationRegionRepository.AllowedRegionsByContract(contract.Idcontract);
                    bool IsContractWithAllowedRegion = regions.Contains(request.RegionId) || !regions.Any();

                    if (IsContractWithAllowedRegion)
                    {
                        contractsRegionAllowed.Add(contract);
                    }
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
                                var unitsToUse = unitsAvailable.Where(ua => ua.Units > 0).FirstOrDefault();
                                if (unitsToUse != null)
                                {
                                    contractToUse.IdJobVacType = (int)unitsToUse.type;
                                }
                                break;
                            }
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
                        }
                    }
                }
                if (contractToUse.Idcontract == 0)
                    return ContractResult.Failure(new List<string> { "There is no contracts / units or regions assigned available." });
                else
                    return ContractResult.Success(await Task.FromResult(contractToUse));
            }
        }
    }
}
