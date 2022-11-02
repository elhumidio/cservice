using Application.Aimwel.Interfaces;
using Application.Core;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;

namespace Application.Aimwel.Commands
{
    public class Create
    {
        public class Command : IRequest<Result<CreateCampaignResponse>>
        {
            public int offerId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<CreateCampaignResponse>>
        {
            private readonly IJobOfferRepository _offerRepo;

            private readonly IAimwelCampaign _manageCampaign;

            public Handler(IJobOfferRepository offerRepo,
                IAimwelCampaign aimwelCampaign)
            {
                _offerRepo = offerRepo;
                _manageCampaign = aimwelCampaign;
            }

            public async Task<Result<CreateCampaignResponse>> Handle(Command request, CancellationToken cancellationToken)
            {
                var job = _offerRepo.GetOfferById(request.offerId);
                if (job == null) return null;
                else
                {
                    var campaign = await _manageCampaign.CreateCampaing(job);
                    return Result<CreateCampaignResponse>.Success(campaign);
                }
            }
        }
    }
}
