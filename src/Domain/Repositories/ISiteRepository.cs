using Domain.Entities;

namespace Domain.Repositories
{
    public interface ISiteRepository
    {
        public IQueryable<Site> GetSites();
    }
}
