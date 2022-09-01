using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class RegJobVacMatchingRepository : IRegJobVacMatchingRepository
    {
        DataContext _dataContext;

        public RegJobVacMatchingRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public Task<int> Add(RegJobVacMatching recjob)
        {
            var a = _dataContext.Add(recjob).Entity;
            var ret = _dataContext.SaveChangesAsync();
            return ret;
        }

        public Task<RegJobVacMatching> GetAtsIntegrationInfo(string externalId)
        {
            var obj = _dataContext.RegJobVacMatchings.Where(integration => integration.ExternalId == externalId);
            if (obj != null && obj.Any())
                return Task.FromResult(obj.First());
            else return Task.FromResult<RegJobVacMatching>(null);
        }

    }
}
