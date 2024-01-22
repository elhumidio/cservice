using Application.Core;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.OnlineShop.Commands
{
    public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, Result<bool>>
    {
        private readonly IContractPaymentRepository _contractPaymentRepository;
        private readonly IContractRepository _contractRepository;

        public AddPaymentCommandHandler(IContractPaymentRepository contractPaymentRepository, IContractRepository contractRepository)
        {
            _contractPaymentRepository = contractPaymentRepository; 
            _contractRepository = contractRepository;
        }

        public async Task<Result<bool>> Handle(AddPaymentCommand request, CancellationToken cancellationToken)
        {
            //TODO see if divide by 100 is needed
            bool ret = false;
            var contract = await _contractRepository.GetContractByStripeSessionId(request.SessionId);
            _ = await _contractRepository.UpdateContract(contract);
            var ent = new ContractPayment
            {
                Idcontract = contract.Idcontract,
                DataPayment = DateTime.Now,
                Payment = request.amount_total,
                PaymentWithoutTax = Convert.ToDecimal(request.amount_subtotal),
                CouponDiscount = Convert.ToDecimal(request.amount_discount),
                TaxAmount = Convert.ToDecimal(request.amount_tax)
            };
            try
            {
                ret = await _contractPaymentRepository.AddPayment(ent);
            }
            catch (Exception ex) {
                var a = ex;
                return Result<bool>.Failure("Couldn't add payment");
                
            }
            if (ret)
            {
                return Result<bool>.Success(true);
            }
            else
            {
                return Result<bool>.Failure("Couldn't add payment");
            }
        }
    }
}
