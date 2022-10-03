using Application.JobOffer.Commands;
using Application.JobOffer.DTO;
using DPGRecruitmentCampaignClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Aimwel.Interfaces
{
    public interface IAimwelCampaign
    {
        public Task<CreateCampaignResponse> CreateCampaing(CreateOfferCommand job);
        public Task<GetCampaignResponse> GetCampaign(GetCampaignRequest request);
    }
}
