using Application.Contracts.DTO;
using Application.Core;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories;
using MediatR;

namespace Application.EnterpriseContract.Queries
{
    public class GetContract
    {


        public class Query : IRequest<Result<ContractDTO>>
        {
            public int CompanyId { get; set; }
            public VacancyType? type { get; set; }

        }
        public class Handler : IRequestHandler<Query, Result<ContractDTO>>
        {
            IMediator _mediatr;
            private readonly IContractRepository _contractRepository;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IContractRepository contractRepository, IMediator mediatr)
            {
                _mapper = mapper;
                _contractRepository = contractRepository;
                _mediatr = mediatr;
            }

            public async Task<Result<ContractDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var ret = new ContractDTO();
                var contracts = _mediatr.Send(new Application.Contracts.Queries.List.Query
                {
                    CompanyId = request.CompanyId,


                }).Result;
                foreach (var (contract, type) in from contract in contracts.Value
                                                 from type in contract.RegEnterpriseContracts
                                                 select (contract, type))
                {
                    if (request.type == VacancyType.None)
                    {
                        if ((type.Units - type.UnitsUsed) > 0)
                        {
                            ret = contract;
                            ret.IdJobVacType = type.IdjobVacType;
                            break;
                        }
                    }
                    else
                    {
                        var canUse = (request.type == (VacancyType)type.IdjobVacType) && (type.Units - type.UnitsUsed > 0);
                        if (canUse)
                        {
                            ret = contract;
                            ret.IdJobVacType = type.IdjobVacType;
                            break;

                        }
                    }
                }

                return Result<ContractDTO>.Success(await Task.FromResult(ret));
            }
        }
    }
}
