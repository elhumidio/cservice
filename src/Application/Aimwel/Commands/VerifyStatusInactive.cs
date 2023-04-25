using Application.Aimwel.Interfaces;
using Application.Core;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Aimwel.Commands
{
    public class VerifyStatusInactive
    {
        public class Query : IRequest<Result<bool>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<bool>>
        {
            private readonly IJobOfferRepository _jobOfferRepository;
            private readonly IAimwelCampaign _manageCampaign;
            private readonly ILogger<Handler> _logger;

            public Handler(IJobOfferRepository jobOffer, IAimwelCampaign aimwelCampaign, ILogger<Handler> logger)
            {
                _jobOfferRepository = jobOffer;
                _manageCampaign = aimwelCampaign;
                _logger = logger;
            }

            public async Task<Result<bool>> Handle(Query request, CancellationToken cancellationToken)
            {
                var inactiveOffers = await _jobOfferRepository.GetInactiveOffersChunk();
                foreach (var offer in inactiveOffers)
                {
                    var campaign = await _manageCampaign.GetCampaignState(offer.IdjobVacancy);
                    await _manageCampaign.StopCampaign(offer.IdjobVacancy);
                    _logger.LogInformation("CANCEL CAMPAIGN - OFFER ENDED", new { campaign.CampaignId, offer.IdjobVacancy });
                }
                return Result<bool>.Success(true);
            }
        }
    }
}
