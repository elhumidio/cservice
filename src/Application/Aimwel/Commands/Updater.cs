using Application.Aimwel.Interfaces;
using Application.JobOffer.DTO;
using Domain.Entities;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                //List<JobOfferDto> activeOffers = new List<JobOfferDto>();
                //activeOffers = _offerRepo.GetActiveOffers().ToList();


                var offersList = _offerRepo.GetActiveOffers().ToList();
                foreach (JobVacancy offer in offersList)
                //foreach (JobOfferDto offer in offersList)
                {
                    bool cancelCampaign = await _manageCampaign.StopCampaign(offer.IdjobVacancy);
                    var campaignCreated = await _manageCampaign.CreateCampaing(offer);
                }

                return new Response();
            }
        }

    }
}
