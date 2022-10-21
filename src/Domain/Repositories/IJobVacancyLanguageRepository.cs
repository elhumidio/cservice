using Domain.Entities;

namespace Domain.Repositories
{
    public interface IJobVacancyLanguageRepository
    {
        public IQueryable<JobVacancyLanguage> Get(int _idJob);

        public bool Delete(int _idJob);

        public bool Add(JobVacancyLanguage _lang);
    }
}
