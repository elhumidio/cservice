using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRegJobVacMatchingRepository
    {
        public Task<int> Add(RegJobVacMatching recjob);
        public Task<RegJobVacMatching> GetAtsIntegrationInfo(string externalId);
    }
}
