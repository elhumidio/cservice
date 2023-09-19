using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Commands
{
    public class VerifyOfferCommsHandler : IRequestHandler<VerifyOfferCommsCommand, Result<bool>>
    {
        private readonly IJobOfferRepository _jobOfferRepository;

        public VerifyOfferCommsHandler(IJobOfferRepository jobOfferRepository)
        {
            _jobOfferRepository = jobOfferRepository;
        }

        public async Task<Result<bool>> Handle(VerifyOfferCommsCommand request, CancellationToken cancellationToken)
        {
            return Result<bool>.Success(await _jobOfferRepository.OfferAllowCommns(request.Offerid));
        }
    }
}
