using Domain.Entities;

namespace Domain.Repositories
{
    public interface IJobCategoryRepository
    {
        public bool IsRightCategory(int? jobCatId);

        public IQueryable<JobCategory> GetJobCategories(int siteId, int languageId);

        public JobCategory GetJobCategoryById(int jobCategoryId, int siteId, int languageId);
    }
}
