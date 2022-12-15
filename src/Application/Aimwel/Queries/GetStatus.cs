using Application.Aimwel.Interfaces;
using Application.JobOffer.DTO;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;

namespace Application.Aimwel.Queries
{
    public class GetStatus
    {
        public class Query : IRequest<GetCampaignResponse>
        {
            public int OfferId { get; set; }
        }

        public class Handler : IRequestHandler<Query, GetCampaignResponse>
        {
            private readonly IJobOfferRepository _offerRepo;
            private readonly IAimwelCampaign _manageCampaign;

            public Handler(IJobOfferRepository jobOfferRepository, IAimwelCampaign aimwelCampaign)
            {
                _offerRepo = jobOfferRepository;
                _manageCampaign = aimwelCampaign;
            }

            public async Task<GetCampaignResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = await _manageCampaign.GetCampaignState(request.OfferId);
                return response;
            }
        }
    }
}
