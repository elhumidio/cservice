using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class EnterpriseUserJobVacRepository : IEnterpriseUserJobVacRepository
    {
        private readonly DataContext _dataContext;

        public EnterpriseUserJobVacRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<int> Add(EnterpriseUserJobVac ujobvac)
        {
            var ret = await _dataContext.AddAsync(ujobvac);
            return ret.Entity.Idcontract;
        }
    }
}
