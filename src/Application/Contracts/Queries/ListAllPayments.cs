using Application.Core;
using AutoMapper;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class ListAllPayments
    {
        public class Query : IRequest<Result<List<ContractPaymentDto>>>
        {
            public int ContractId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ContractPaymentDto>>>
        {
            private readonly IContractPaymentRepository _contractPaymentRepo;
            private readonly IMapper _mapper;

            public Handler(IContractPaymentRepository contractRepo, IMapper mapper)
            {
                _contractPaymentRepo = contractRepo;
                _mapper = mapper;
            }

            public async Task<Result<List<ContractPaymentDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = await _contractPaymentRepo.GetPaymentsByContractId(request.ContractId);
                if (query != null)
                {
                    return Result<List<ContractPaymentDto>>.Success(query);
                }
                return Result<List<ContractPaymentDto>>.Failure("Couldn't retrieve payments");
            }
        }
    }
}
