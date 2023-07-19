using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class JobExpYearsRepository : IJobExpYearsRepository
    {
        private DataContext _dataContext;

        public JobExpYearsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool IsRightExperienceYears(int _experienceYearsId)
        {
            var jobexp = _dataContext.JobExpYears.Where(exp => exp.IdjobExpYears == _experienceYearsId);
            return jobexp.Any();
        }

        public IQueryable<JobExpYear> GetJobExperienceYears(int siteId, int languageId)
        {
            var jobExperienceYears = _dataContext.JobExpYears
                .Where(a => a.Idsite == siteId && a.Idslanguage == languageId);

            if (jobExperienceYears != null)
            {
                return jobExperienceYears;
            }
            else
            {
                return null;
            }
        }

        public JobExpYear GetJobExperienceYearsById(int jobExperienceId, int siteId, int languageId)
        {
            var jobExperienceYears = _dataContext.JobExpYears
                .FirstOrDefault(a => a.IdjobExpYears == jobExperienceId && a.Idsite == siteId && a.Idslanguage == languageId);

            if (jobExperienceYears != null)
            {
                return jobExperienceYears;
            }
            else
            {
                return null;
            }
        }
    }
}
