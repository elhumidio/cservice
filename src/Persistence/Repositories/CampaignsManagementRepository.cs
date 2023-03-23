using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class CampaignsManagementRepository : ICampaignsManagementRepository
    {
        private readonly DataContext _dataContext;

        public CampaignsManagementRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<int> Add(CampaignsManagement _campaign)
        {
            try
            {
                var a = _dataContext.Add(_campaign).Entity;
                var ret = await _dataContext.SaveChangesAsync();
                return ret;
            }
            catch (Exception ex)
            {
                var a = ex;
                return -1;
            }
        }

        public int AddRange(List<CampaignsManagement> _campaigns)
        {
            try
            {
                _dataContext.CampaignManagements.AddRange(_campaigns);
                var ret = _dataContext.SaveChanges();
                return ret;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<CampaignSetting> GetCampaignSetting(JobVacancy job)
        {
            var settings = _dataContext.CampaignSettings.FirstOrDefault(j => j.AreaId == job.Idarea && j.RegionId == job.Idregion && j.SiteId == job.Idsite);
            return settings;
        }

        public async Task<string> GetAimwellIdByJobId(int _jobId)
        {
            var campaignData = await _dataContext.CampaignManagements.Where(j => j.IdjobVacancy == _jobId).OrderByDescending(d => d.LastModificationDate).FirstOrDefaultAsync();
            if (campaignData == null)
                return null;
            else return campaignData.ExternalCampaignId;
        }

        public async Task<CampaignsManagement> GetCampaignManagement(int _jobId)
        {
            var campaign = await _dataContext.CampaignManagements.Where(j => j.IdjobVacancy == _jobId).OrderByDescending(d => d.LastModificationDate).FirstOrDefaultAsync();
            return campaign;
        }

        public async Task<bool> Update(CampaignsManagement _campaign)
        {
            try
            {
                _dataContext.CampaignManagements.Update(_campaign);
                var ret = await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var a = ex;
            }

            return true;
        }


        public async Task<bool> GetCampaignNeedsUpdate(string campaignName)
        {
            var campaign = await _dataContext.CampaignsUpdatingChecks.Where(c => c.Campaign == campaignName).FirstOrDefaultAsync();
            if(campaign != null)
            return  (bool)campaign.NeedsRefresh;
            else return false;  
        }
             
        public async Task<bool> MarkCampaignUpdated(string campaignName)
        {
            var campaign = await _dataContext.CampaignsUpdatingChecks.Where(c => c.Campaign == campaignName).FirstOrDefaultAsync();
            if (campaign == null) return false;
            else
            {
                campaign.NeedsRefresh = false;
                var a = await _dataContext.SaveChangesAsync();
                return a > 0;
            }
        }
    }
}
