using API.DataContext;

namespace Persistence.Repositories
{
    public interface IRegJobVacMatchingRepository
    {
        public Task<int> Add(RegJobVacMatching recjob);
        public Task<RegJobVacMatching> Exists(string externalId);
    }
}
