using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Persistence.Repositories
{
    public class RegJobVacMatchingRepository : IRegJobVacMatchingRepository
    {
        private DataContext _dataContext;

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

        public async Task<bool> Update(RegJobVacMatching recjob)
        {
            _dataContext.RegJobVacMatchings.Update(recjob);
            var ret = await _dataContext.SaveChangesAsync();
            return true;
        }

        public Task<RegJobVacMatching> GetAtsIntegrationInfo(string externalId)
        {
            var obj = _dataContext.RegJobVacMatchings.Where(integration => integration.ExternalId == externalId).OrderByDescending(i => i.IdjobVacancy);
            if (obj != null && obj.Any())
                return Task.FromResult(obj.First());
            else return Task.FromResult<RegJobVacMatching>(null);
        }

        public Task<RegJobVacMatching> GetAtsIntegrationInfoByJobId(int id)
        {
            var obj = _dataContext.RegJobVacMatchings.Where(integration => integration.IdjobVacancy == id).OrderByDescending(i => i.IdjobVacancy);
            if (obj != null && obj.Any())
                return Task.FromResult(obj.First());
            else return Task.FromResult<RegJobVacMatching>(null);
        }

        public Task<List<RegJobVacMatching>> GetAtsIntegrationInfoForFile(string externalId)
        {
            var obj = _dataContext.RegJobVacMatchings.Where(integration => integration.ExternalId == externalId).OrderByDescending(i => i.IdjobVacancy).ToList();
            if (obj != null && obj.Any())
                return Task.FromResult(obj);
            else return Task.FromResult<List<RegJobVacMatching>>(null);
        }
    }
}
