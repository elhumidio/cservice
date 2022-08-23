using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly DataContext _dataContext;
        public SiteRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<Site> GetSites()
        {
            var items = _dataContext.Sites;

            if(items != null)
            {
                return items;
            }
            else
            {
                return null;
            }
        }
    }
}
