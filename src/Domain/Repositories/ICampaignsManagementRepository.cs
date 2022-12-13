using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICampaignsManagementRepository
    {
        public Task<int> Add(CampaignsManagement campaign);
        public Task<bool> Update(CampaignsManagement _campaign);
        public Task<CampaignSetting> GetCampaignSetting(JobVacancy job);
        public Task<string> GetAimwellIdByJobId(int _jobId);
    }
}

