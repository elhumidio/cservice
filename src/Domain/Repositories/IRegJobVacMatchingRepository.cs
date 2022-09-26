using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRegJobVacMatchingRepository
    {
        public Task<int> Add(RegJobVacMatching recjob);

        public Task<RegJobVacMatching> GetAtsIntegrationInfo(string externalId);
        public Task<List<RegJobVacMatching>> GetAtsIntegrationInfoForFile(string externalId);
        public Task<RegJobVacMatching> GetAtsIntegrationInfoByJobId(int id);
    }
}
