using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class JobVacTypeRepository : IJobVacTypeRepository
    {
        private readonly DataContext _dataContext;

        public JobVacTypeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<JobVacType> GetJobVacTypes(int siteId, int languageId)
        {
            var jobVacTypes = _dataContext.JobVacTypes
                .Where(a => a.Idsite == siteId && a.Idslanguage == languageId);

            if (jobVacTypes != null)
            {
                return jobVacTypes;
            }
            else
            {
                return null;
            }
        }
    }
}
