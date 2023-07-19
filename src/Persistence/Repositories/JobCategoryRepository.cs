using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class JobCategoryRepository : IJobCategoryRepository
    {
        private DataContext _dataContext;

        public JobCategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool IsRightCategory(int? _jobCatId)
        {
            var jobCat = _dataContext.JobCategories.Where(jc => jc.IdjobCategory == _jobCatId);
            return jobCat.Any();
        }

        public IQueryable<JobCategory> GetJobCategories(int siteId, int languageId)
        {
            var jobCategories = _dataContext.JobCategories
                .Where(a => a.Idsite == siteId && a.Idslanguage == languageId)
                .Where(a => a.IdjobCategory > 0);

            if (jobCategories != null)
            {
                return jobCategories;
            }
            else
            {
                return null;
            }
        }

        public JobCategory GetJobCategoryById(int jobCategoryId, int siteId, int languageId)
        {
            var jobCategories = _dataContext.JobCategories
                .FirstOrDefault(a => a.IdjobCategory == jobCategoryId && a.Idsite == siteId && a.Idslanguage == languageId);

            if (jobCategories != null)
            {
                return jobCategories;
            }
            else
            {
                return null;
            }
        }
    }
}
