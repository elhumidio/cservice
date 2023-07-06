using Domain.Entities;

namespace Domain.Repositories
{
    public interface ICampaignsManagementRepository
    {
        public Task<int> Add(CampaignsManagement campaign);
        public Task<bool> Update(CampaignsManagement _campaign);
        public Task<CampaignSetting> GetCampaignSetting(JobVacancy job);
        public Task<string> GetAimwellIdByJobId(int _jobId);
        public Task<CampaignsManagement> GetCampaignManagement(int _jobId);
        public Task<List<CampaignsManagement>> GetAllCampaignManagement(int _jobId);
        public Task<bool> MarkCampaignUpdated(string campaign);
        public Task<bool> GetCampaignNeedsUpdate(string campaignName);
        public int AddRange(List<CampaignsManagement> _campaigns);
        public IQueryable<CampaignSetting> GetAllSettings();
        public int GetNextId();
        public Task<bool> SaveFeedLogs(List<FeedsAggregatorsLog> logList);
    }
}

