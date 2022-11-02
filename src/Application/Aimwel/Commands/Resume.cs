using Application.Aimwel.Interfaces;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;

namespace Application.Aimwel.Commands
{
    public class Resume
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
                    var campaign = await _manageCampaign.ResumeCampaign(job.IdjobVacancy);
                    return new Response();
                }
            }
        }
    }
}
