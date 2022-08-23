using Domain.Entities;

namespace Domain.Repositories
{
    public interface IDegreeRepository
    {
        public bool IsRightDegree(int degreeId);

        public IQueryable<Degree> GetDegrees(int siteId, int languageId);
    }
}
