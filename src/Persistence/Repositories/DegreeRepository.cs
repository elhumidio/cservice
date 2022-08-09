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
    }
}
