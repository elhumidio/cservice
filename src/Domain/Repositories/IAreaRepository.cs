using Domain.Entities;

namespace Domain.Repositories
{
    public interface IAreaRepository
    {
        public bool IsRightArea(int? areaId);

        public IQueryable<Area> GetAreas(int siteId, int languageId);
        public int GetIscoDefaultFromArea(int _area);
    }
}
