using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class RegEnterpriseConsumsRepository : IRegEnterpriseConsumsRepository
    {
        private DataContext _dataContext;

        public RegEnterpriseConsumsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<int> Add(RegEnterpriseConsum contractConsum)
        {            
            var ret = await _dataContext.Set<RegEnterpriseConsum>().AddAsync(contractConsum);
            return contractConsum.Idcontract;
            
        }
    }
}
