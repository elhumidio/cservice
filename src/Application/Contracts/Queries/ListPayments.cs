using Application.Core;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class ListPayments
    {
        public class Query : IRequest<Result<ContractPayment>>
        {
            public int ContractId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContractPayment>>
        {
            private readonly IContractPaymentRepository _contractPaymentRepo;

            public Handler(IContractPaymentRepository contractRepo)
            {
                _contractPaymentRepo = contractRepo;
            }

            public async Task<Result<ContractPayment>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = await _contractPaymentRepo.GetPaymentByContractId(request.ContractId);
                return Result<ContractPayment>.Success(query);
            }
        }
    }

}
