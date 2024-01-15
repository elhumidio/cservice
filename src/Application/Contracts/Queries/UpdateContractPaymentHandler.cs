using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class UpdateContractPaymentHandler : IRequestHandler<UpdateContractPaymentCommand, Result<bool>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IContractPaymentRepository _contractPaymentRepository;

        public UpdateContractPaymentHandler(IContractPaymentRepository contractPaymentRepository, IContractRepository contractRepository)
        {
            _contractPaymentRepository = contractPaymentRepository;
            _contractRepository = contractRepository;
        }

        public async Task<Result<bool>> Handle(UpdateContractPaymentCommand request, CancellationToken cancellationToken)
        {
            var contract = await _contractRepository.GetContractByStripeSessionId(request.SessionId);
            if (contract != null)
            {
                contract.ChkApproved = true;
                var contractModified = await _contractRepository.UpdateContract(contract);

            }
            
            var contractPayment = await _contractPaymentRepository.GetPaymentByContractId(contract.Idcontract);
            if(contractPayment != null)
            {
                contractPayment.DataPayment = DateTime.Now;
                contractPayment.TaxAmount= request.amount_tax;
                contractPayment.CouponDiscount = request.amount_discount;
                contractPayment.PaymentWithoutTax = request.amount_subtotal;
                contractPayment.Payment = request.amount_total;
                var ans = await _contractPaymentRepository.UpdatePayment(contractPayment);
                if(ans)
                {
                    return Result<bool>.Success(true);
                }
                else
                {
                  return Result<bool>.Failure("Error updating payment");   
                }
            }
            else
            {
                return Result<bool>.Failure("Error updating payment");
            }

        }
    }
}
