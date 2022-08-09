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
    }
}
