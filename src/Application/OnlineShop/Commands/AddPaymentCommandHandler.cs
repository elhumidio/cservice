using Application.Core;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.OnlineShop.Commands
{
    public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, Result<bool>>
    {
        private readonly IContractPaymentRepository _contractPaymentRepository;

        public AddPaymentCommandHandler(IContractPaymentRepository contractPaymentRepository)
        {
            _contractPaymentRepository = contractPaymentRepository;
        }

        public async Task<Result<bool>> Handle(AddPaymentCommand request, CancellationToken cancellationToken)
        {
            var ent = new ContractPayment
            {
                Idcontract = request.Idcontract,
                DataPayment = request.DataPayment,
                Payment = request.Payment,
                PaymentWithoutTax = request.PaymentWithoutTax
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
