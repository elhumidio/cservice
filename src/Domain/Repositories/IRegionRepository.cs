using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRegionRepository
    {
        public bool IsRightRegion(int regionId);

        public Region Get(int _regionId);

        public int GetCountryByRegion(int regionId);

        public IQueryable<Region> GetRegions(int siteId, int languageId);
        public int GetLastRegionId();
        public int GetCountry(int _region);
        public Task<int> Add(Region _region);
        public string GetRegionNameByID(int _region, bool english);
        public string GetCCAAByID(int _region, bool english);
    }
}
