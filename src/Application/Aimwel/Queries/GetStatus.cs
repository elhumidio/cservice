using Application.Aimwel.Interfaces;
using Application.JobOffer.DTO;
using AutoMapper;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            public Handler(IJobOfferRepository jobOfferRepository , IAimwelCampaign aimwelCampaign)
            {
                _offerRepo = jobOfferRepository;
                _manageCampaign = aimwelCampaign;
            }

            public async Task<GetCampaignResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                var jobdto = new JobOfferDto();
                var job = _offerRepo.GetOfferById(request.OfferId);
                var response = await _manageCampaign.GetCampaignState(job.IdjobVacancy);
                return response;
            }
        }
    }
}
