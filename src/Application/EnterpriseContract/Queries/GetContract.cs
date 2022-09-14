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
                var contracts = _mediatr.Send(new List.Query
                {
                    CompanyId = request.CompanyId,
                }).Result;

                foreach (var contract in contracts.Value)
                {
                    var regions = _contractPublicationRegionRepository.AllowedRegionsByContract(contract.Idcontract);
                    if (regions.Contains(request.RegionId))
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
                            var unitsAvailable = units.Sum(u => u.Units);

                            if ((unitsAvailable) > 0)
                            {
                                contractToUse = contract;
                                contractToUse.IdJobVacType = type.IdjobVacType;
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
