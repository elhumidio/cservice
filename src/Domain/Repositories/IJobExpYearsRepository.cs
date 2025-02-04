using Domain.Entities;

namespace Domain.Repositories
{
    public interface IJobExpYearsRepository
    {
        public bool IsRightExperienceYears(int experienceYearsId);

        public IQueryable<JobExpYear> GetJobExperienceYears(int siteId, int languageId);

        public JobExpYear GetJobExperienceYearsById(int jobExperienceId, int siteId, int languageId);
    }
}
