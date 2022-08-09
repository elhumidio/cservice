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
    }
}
