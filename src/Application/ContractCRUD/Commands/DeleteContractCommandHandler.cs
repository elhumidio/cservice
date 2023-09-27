using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.ContractCRUD.Commands
{
    public class DeleteContractCommandHandler : IRequestHandler<DeleteContractCommand, Result<bool>>
    {
        private IContractRepository _contractRepository;
        private IJobOfferRepository _jobOfferRepository;

        public DeleteContractCommandHandler(IJobOfferRepository jobOfferRepository, IContractRepository contractRepository)
        {
            _jobOfferRepository = jobOfferRepository;
            _contractRepository = contractRepository;
        }

        public Task<Result<bool>> Handle(DeleteContractCommand request, CancellationToken cancellationToken)
        {
            //Update the contract to set chkApproved = 0
            var disabledOk = _contractRepository.DisableContract(request.IdContract);

            //Also for each offer in this contract set chkFilled and Filled Date
            var offers = _jobOfferRepository.GetActiveOffersByContract(request.IdContract);
            var filedOk = _jobOfferRepository.FileAllOffers(offers);

            return Task.FromResult(Result<bool>.Success(disabledOk && filedOk.All(i => i > 0)));
        }
    }

}
