using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.ContractCRUD.Commands
{
    public class UpdateStripeSessionCommand : IRequest<Result<bool>>
    {
        public string SessionId { get; set; }
        public int ContractId { get; set; }
    }

    public class UpdateStripeSessionCommandHandler : IRequestHandler<UpdateStripeSessionCommand, Result<bool>>
    {
        private readonly IContractRepository _contractRepository;

        public UpdateStripeSessionCommandHandler(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public async Task<Result<bool>> Handle(UpdateStripeSessionCommand request, CancellationToken cancellationToken)
        {
            var contract = _contractRepository.Get(request.ContractId).FirstOrDefault();

            if (contract != null)
            {
                
                contract.CheckoutSessionId = request.SessionId;
                var contractModified = await _contractRepository.UpdateContract(contract);

                return contractModified
                    ? Result<bool>.Success(true)
                    : Result<bool>.Failure("Error updating contract");
            }

            return Result<bool>.Failure("Error updating contract");
        }
    }
}
