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
            var ent = new ContractPayment
            {
                Idcontract = request.Idcontract,
                DataPayment = request.DataPayment,
                Payment = request.Payment,
                PaymentWithoutTax = request.PaymentWithoutTax
            };

            var ret = await _contractPaymentRepository.AddPayment(ent);
            //TODO approve contract
            var contract =  _contractRepository.Get(request.Idcontract).FirstOrDefault();
            contract.ChkApproved = true;
            await _contractRepository.UpdateContract(contract);
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
