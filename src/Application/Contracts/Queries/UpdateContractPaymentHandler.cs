using Application.Core;
using Domain.Entities;
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

                var contractPayment = await _contractPaymentRepository.GetPaymentByContractId(contract.Idcontract);

                if (contractPayment != null)
                {
                    contractPayment = UpdateProperties(request,contractPayment);
                    var paymentUpdateSuccess = await _contractPaymentRepository.UpdatePayment(contractPayment);

                    return paymentUpdateSuccess
                        ? Result<bool>.Success(true)
                        : Result<bool>.Failure("Error updating payment");
                }
            }

            return Result<bool>.Failure("Error updating payment");



        }

        
        private ContractPayment UpdateProperties(UpdateContractPaymentCommand request,ContractPayment contractPayment)
        {
            contractPayment.DataPayment = DateTime.Now;
            contractPayment.TaxAmount = request.amount_tax;
            contractPayment.CouponDiscount = request.amount_discount;
            contractPayment.PaymentWithoutTax = request.amount_subtotal;
            contractPayment.Payment = request.amount_total;
            return contractPayment;
        }

    }
}
