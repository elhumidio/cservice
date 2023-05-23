using Application.Aimwel.Interfaces;
using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.Aimwel.Commands
{
    public class Cancel
    {
        public class Command : IRequest<Result<bool>>
        {
            public int offerId { get; set; }
            public int? ModificationReason { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<bool>>
        {
            private readonly IAimwelCampaign _manageCampaign;
            private readonly IJobOfferRepository _offerRepo;

            public Handler(
                IAimwelCampaign aimwelCampaign, IJobOfferRepository jobOfferRepository)
            {
                _manageCampaign = aimwelCampaign;
            }

            public async Task<Result<bool>> Handle(Command request, CancellationToken cancellationToken)
            {
                var job = _offerRepo.GetOfferById(request.offerId);
                var response = await _manageCampaign.StopCampaign(job, request.ModificationReason);
                return Result<bool>.Success(response);
            }
        }
    }
}
