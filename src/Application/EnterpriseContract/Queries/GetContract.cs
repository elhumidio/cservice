using Application.Contracts.DTO;
using Application.Contracts.Queries;
using Application.Core;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories;
using MediatR;

namespace Application.EnterpriseContract.Queries
{
    public class GetContract
    {
        public class Query : IRequest<Result<ContractDto>>
        {
            public int CompanyId { get; set; }
            public VacancyType? type { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContractDto>>
        {
            private IMediator _mediatr;
            private readonly IContractRepository _contractRepository;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IContractRepository contractRepository, IMediator mediatr)
            {
                _mapper = mapper;
                _contractRepository = contractRepository;
                _mediatr = mediatr;
            }

            public async Task<Result<ContractDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var contractToUse = new ContractDto();
                var contracts = _mediatr.Send(new List.Query
                {
                    CompanyId = request.CompanyId,
                }).Result;

                foreach (var (contract, type) in from contract in contracts.Value
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
                if (contractToUse.Idcontract == 0)
                    return Result<ContractDto>.Failure("There is no contracts or units available.\n");
                else
                    return Result<ContractDto>.Success(await Task.FromResult(contractToUse));
            }
        }
    }
}
