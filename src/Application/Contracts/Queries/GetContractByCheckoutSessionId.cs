using Application.Contracts.DTO;
using Application.Core;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class GetContractByCheckoutSessionId
    {
        public class Query : IRequest<Result<ContractShortDto>>
        {
            public string SessionId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContractShortDto>>
        {
            private readonly IContractRepository _contractRepo;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IContractRepository contractRepo)
            {
                _mapper = mapper;
                _contractRepo = contractRepo;
            }

            public async Task<Result<ContractShortDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var contract = await _contractRepo.GetContractByStripeSessionId(request.SessionId);
                var dto = _mapper.Map<ContractShortDto>(contract);
                return Result<ContractShortDto>.Success(dto);
            }
        }
    }
}
