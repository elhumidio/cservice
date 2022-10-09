using Application.JobOffer.Commands;
using DPGRecruitmentCampaignClient;

namespace Application.Aimwel.Interfaces
{
    public interface IAimwelCampaign
    {
        public Task<CreateCampaignResponse> CreateCampaing(CreateOfferCommand job);

        public Task<GetCampaignResponse> GetCampaign(GetCampaignRequest request);
        public Task<bool> StopAimwelCampaign(int jobId);
    }
}
