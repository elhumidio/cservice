using Domain.Repositories;

namespace Persistence.Repositories
{
    public class JobExpYearsRepository : IJobExpYearsRepository
    {
        DataContext _dataContext;
        public JobExpYearsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool IsRightExperienceYears(int _experienceYearsId)
        {
            var jobexp = _dataContext.JobExpYears.Where(exp => exp.IdjobExpYears == _experienceYearsId);
            return jobexp.Any();
        }
    }
}
