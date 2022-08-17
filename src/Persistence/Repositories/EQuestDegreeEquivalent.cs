using Domain.Enums;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class EQuestDegreeEquivalentRepository : IEQuestDegreeEquivalentRepository
    {
        private readonly DataContext _dataContext;
        public EQuestDegreeEquivalentRepository(DataContext dataContext)
        {

            _dataContext = dataContext;
        }
        public Task<int> GeteQuestDegree(int degreeId)
        {
            var degree = _dataContext.EquestDegreeEquivalents.Where(d => d.IdequestDegree == degreeId && d.Idslanguage == (int)Languages.Spanish).FirstOrDefault();
            if (degree != null)
                return Task.FromResult(degree.Iddegree);
            else return Task.FromResult(-1);
        }
    }
}
