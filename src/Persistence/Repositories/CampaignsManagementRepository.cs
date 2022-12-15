using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CampaignsManagementRepository : ICampaignsManagementRepository
    {
        private readonly DataContext _dataContext;
        public CampaignsManagementRepository(DataContext dataContext) {
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

        
    }
}
