using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class DegreeRepository : IDegreeRepository
    {
        private readonly DataContext _dataContext;

        public DegreeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool IsRightDegree(int _degreeId)
        {
            var degree = _dataContext.Degrees.Where(d => d.Iddegree == _degreeId);
            return degree.Any();
        }

        public IQueryable<Degree> GetDegrees(int siteId, int languageId)
        {
            var degrees = _dataContext.Degrees
                .Where(a => a.Idsite == siteId && a.Idslanguage == languageId)
                .Where(a => a.Iddegree > 0);

            if (degrees != null)
            {
                return degrees;
            }
            else
            {
                return null;
            }
        }

        public Degree GetDegreeById(int degreeId, int siteId, int languageId)
        {
            var degree = _dataContext.Degrees.FirstOrDefault(x => x.Iddegree == degreeId && x.Idsite == siteId && x.Idslanguage == languageId);
            if (degree == null)
            {
                return null;
            }
            return degree;
        }
    }
}
