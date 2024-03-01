using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class FinishPayments
    {
        public class Query : IRequest<Result<bool>>
        {
            public int ContractId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<bool>>
        {
            private readonly IContractPaymentRepository _contractPaymentRepo;

            public Handler(IContractPaymentRepository contractRepo)
            {
                _contractPaymentRepo = contractRepo;
            }

            public async Task<Result<bool>> Handle(Query request, CancellationToken cancellationToken)
            {
                var payment = await _contractPaymentRepo.GetPaymentByContractId(request.ContractId);
                payment.Finished = true;
                var updated = await _contractPaymentRepo.UpdatePayment(payment);                
                return Result<bool>.Success(updated);
            }
        }
    }

}
