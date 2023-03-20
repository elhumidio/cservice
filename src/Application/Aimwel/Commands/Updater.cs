using Application.Aimwel.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;

namespace Application.Aimwel.Commands
{
    public class Updater
    {
        public class Command : IRequest<Response>
        {
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IJobOfferRepository _offerRepo;
            private readonly IAimwelCampaign _manageCampaign;

            public Handler(IJobOfferRepository offerRepo, IAimwelCampaign aimwelCampaign)
            {
                _offerRepo = offerRepo;
                _manageCampaign = aimwelCampaign;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var needUpdate = await _manageCampaign.GetCampaignNeedsUpdate("aimwel");
                if (needUpdate)
                {
                    var offersList = _offerRepo.GetActiveOffers().ToList();
                    foreach (JobVacancy offer in offersList)
                    {
                        bool cancelCampaign = await _manageCampaign.StopCampaign(offer.IdjobVacancy);
                        var campaignCreated = await _manageCampaign.CreateCampaing(offer);
                        
                    }
                    bool updateMarked = await _manageCampaign.MarkUpdateCampaign("aimwel");
                }
                return new Response();
            }
        }
    }
}
