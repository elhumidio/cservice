using Domain.Repositories;

namespace Persistence.Repositories
{
    public class JobCategoryRepository : IJobCategoryRepository
    {
        DataContext _dataContext;

        public JobCategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public bool IsRightCategory(int? _jobCatId)
        {
            var jobCat = _dataContext.JobCategories.Where(jc => jc.IdjobCategory == _jobCatId);
            return jobCat.Any();
        }
    }
}
