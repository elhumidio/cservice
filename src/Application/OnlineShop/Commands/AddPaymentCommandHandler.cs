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
            Contract contract;
            bool ret = false;
            if(string.IsNullOrEmpty(request.SessionId))
            {
                //get contract by id

                contract = _contractRepository.Get(request.ContractId).FirstOrDefault();
                contract.CheckoutSessionId = "Old_Invalid_SessionID";
                if (contract != null)
                {
                    _ = await _contractRepository.UpdateContract(contract);
                    var ent = new ContractPayment
                    {
                        Idcontract = contract.Idcontract,
                        DataPayment = DateTime.Now,
                        Payment = request.amount_subtotal == 0 ? (Convert.ToDecimal(request.amount_total)) : (Convert.ToDecimal(request.amount_subtotal)) ,
                        PaymentWithoutTax = (Convert.ToDecimal(request.amount_subtotal)),
                        CouponDiscount = Convert.ToDecimal(request.amount_discount),
                        TaxAmount = Convert.ToDecimal(request.amount_tax),
                        Currency = request.Currency,
                        ConvertRate=0
                    };
                    try
                    {
                        ret = await _contractPaymentRepository.AddPayment(ent);
                    }
                    catch (Exception ex)
                    {
                        var a = ex;
                        return Result<bool>.Failure("Couldn't add payment");

                    }
                }
            }
            else
            {
                contract = await _contractRepository.GetContractByStripeSessionId(request.SessionId);
                _ = await _contractRepository.UpdateContract(contract);
                var ent = new ContractPayment
                {
                    Idcontract = contract.Idcontract,
                    DataPayment = DateTime.Now,
                    Payment = (request.amount_total / 100),
                    PaymentWithoutTax = (Convert.ToDecimal(request.amount_subtotal)),
                    CouponDiscount = Convert.ToDecimal(request.amount_discount),
                    TaxAmount = Convert.ToDecimal(request.amount_tax),
                    Currency = request.Currency,
                    ConvertRate = 0
                };
                try
                {
                    ret = await _contractPaymentRepository. AddPayment(ent);
                }
                catch (Exception ex)
                {
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
            return Result<bool>.Success(ret);
        }
    }
}
