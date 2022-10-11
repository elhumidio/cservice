using Application.Aimwel.Interfaces;
using Application.Core;
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
    public class Pause
    {

        public class Command : IRequest<Response>
        {
            public int offerId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IJobOfferRepository _offerRepo;

            private readonly IAimwelCampaign _manageCampaign;

            public Handler(IJobOfferRepository offerRepo,
                IAimwelCampaign aimwelCampaign)
            {
                _offerRepo = offerRepo;
                _manageCampaign = aimwelCampaign;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var job = _offerRepo.GetOfferById(request.offerId);
                if (job == null) return null;
                else
                {
                    var campaign = await _manageCampaign.PauseCampaign(job.IdjobVacancy);
                    return new Response();
                }
            }
        }
    }
}
