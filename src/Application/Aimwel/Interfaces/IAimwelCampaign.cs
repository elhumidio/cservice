using Domain.Entities;
using DPGRecruitmentCampaignClient;

namespace Application.Aimwel.Interfaces
{
    public interface IAimwelCampaign
    {
        public Task<CreateCampaignResponse> CreateCampaing(JobVacancy job);

        public Task<GetCampaignResponse> GetCampaign(GetCampaignRequest request);

        public Task<bool> StopCampaign(int jobId);

        public Task<bool> PauseCampaign(int jobId);

        public Task<bool> ResumeCampaign(int jobId);
        public Task<bool> GetCampaignNeedsUpdate(string campaignName);

        public Task<GetCampaignResponse> GetCampaignState(int jobId);
        public Task<bool> MarkUpdateCampaign(string campaignName);
        public Task<CampaignsManagement> CreateCampaingUpdater(JobVacancy job);
    }
}
