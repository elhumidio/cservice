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
            var contract = await _contractRepository.GetContractByStripeSessionId(request.SessionId);
            _ = await _contractRepository.UpdateContract(contract);
            var ent = new ContractPayment
            {
                Idcontract = contract.Idcontract,
                DataPayment = DateTime.Now,
                Payment = request.amount_total,
                PaymentWithoutTax = request.amount_subtotal,
                CouponDiscount = request.amount_discount,
                TaxAmount = request.amount_tax
            };

            var ret = await _contractPaymentRepository.AddPayment(ent);
            
           
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
