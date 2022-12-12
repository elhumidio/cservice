using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly DataContext _dataContext;

        public RegionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool IsRightRegion(int _regionId)
        {
            var region = _dataContext.Regions.Where(r => r.Idregion == _regionId && r.ChkActive == 1).FirstOrDefault();
            return region != null || _regionId == (int)Regions.AllCountry;
        }

        public Region Get(int _regionId)
        {
            var region = _dataContext.Regions.Where(r => r.Idregion == _regionId && r.ChkActive == 1).FirstOrDefault();
            return region;
        }

        public async Task<int> Add(Region _region)
        {
            try
            {
                var a = _dataContext.Add(_region).Entity;
                var ret = await _dataContext.SaveChangesAsync();
                return ret;
            }
            catch (Exception ex)
            {   
                return -1;
            }
        }

        public int GetCountry(int _regionId)
        {
            var country = _dataContext.Regions.FirstOrDefault(r => r.Idregion == _regionId && r.ChkActive == 1).Idcountry;
            return country;
        }
        public int GetCountryByRegion(int regionId) {
            int countryId = -1;
            var country = _dataContext.Regions.Where(r => r.Idregion == regionId && r.ChkActive == 1).FirstOrDefault();
            if(country != null)
                countryId = country.Idcountry;
            return countryId;
        }

        public IQueryable<Region> GetRegions(int siteId, int languageId)
        {
            var regions = _dataContext.Regions
                .Where(a => a.Idsite == siteId && a.Idslanguage == languageId)
                .Where(a => a.Idregion > 0)
                .Where(a => a.ChkActive == 1);

            if (regions != null)
            {
                return regions;
            }
            else
            {
                return null;
            }
        }
    }
}
