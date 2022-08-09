using API.DataContext;

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

    }
}
