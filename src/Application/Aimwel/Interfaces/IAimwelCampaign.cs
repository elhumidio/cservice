using Application.JobOffer.Commands;
using Domain.Entities;
using DPGRecruitmentCampaignClient;

namespace Application.Aimwel.Interfaces
{
    public interface IAimwelCampaign
    {
        public Task<CreateCampaignResponse> CreateCampaing(JobVacancy job);

        public Task<GetCampaignResponse> GetCampaign(GetCampaignRequest request);
        public Task<bool> StopCampaign(int jobId);
    }
}
