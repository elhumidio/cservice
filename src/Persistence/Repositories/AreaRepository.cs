using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly DataContext _dataContext;

        public AreaRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool IsRightArea(int? _areaId)
        {
            var area = _dataContext.Areas.Where(a => a.Idarea == _areaId);
            return area.Any();
        }

        public IQueryable<Area> GetAreas(int siteId, int languageId)
        {
            var areas = _dataContext.Areas
                .Where(a => a.Idsite == siteId && a.Idslanguage == languageId)
                .Where(a => a.Idarea > 0 || a.Idarea == -1)
                .Where(a => a.ChkActive == 1);

            if (areas != null)
            {
                return areas;
            }
            else
            {
                return null;
            }
        }
    }
}
