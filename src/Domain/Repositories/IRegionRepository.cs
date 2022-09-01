using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRegionRepository
    {
        public bool IsRightRegion(int regionId);
        public Region Get(int _regionId);
        public IQueryable<Region> GetRegions(int siteId, int languageId);
    }
}
