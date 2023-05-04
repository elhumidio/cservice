using Application.Aimwel.Interfaces;
using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.Aimwel.Commands
{
    public class CreateUpdater
    {
        public class Command : IRequest<Result<Domain.Entities.CampaignsManagement>>
        {
            public int offerId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Domain.Entities.CampaignsManagement>>
        {
            private readonly IJobOfferRepository _offerRepo;

            private readonly IAimwelCampaign _manageCampaign;

            public Handler(IJobOfferRepository offerRepo,
                IAimwelCampaign aimwelCampaign)
            {
                _offerRepo = offerRepo;
                _manageCampaign = aimwelCampaign;
            }

            public async Task<Result<Domain.Entities.CampaignsManagement>> Handle(Command request, CancellationToken cancellationToken)
            {
                var job = _offerRepo.GetOfferById(request.offerId);
                if (job == null) return null;
                else
                {
                    var campaign = await _manageCampaign.CreateCampaingUpdater(job);
                    return Result<Domain.Entities.CampaignsManagement>.Success(campaign);
                }
            }
        }
    }
}
