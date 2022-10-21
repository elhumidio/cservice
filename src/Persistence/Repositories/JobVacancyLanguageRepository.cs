using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories
{
    public class JobVacancyLanguageRepository : IJobVacancyLanguageRepository
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<JobVacancyLanguageRepository> _logger;

        public JobVacancyLanguageRepository(DataContext dataContext, ILogger<JobVacancyLanguageRepository> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public bool Add(JobVacancyLanguage _lang)
        {
            try
            {
                var a = _dataContext.Add(_lang).Entity;
                var ret = _dataContext.SaveChanges();
                return ret > 0 ? true : false;
            }
            catch (Exception ex)
            {
                string message = $"Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}";
                _logger.LogError(message: message);
                return false;
            }
        }

        public bool Delete(int _idJob)
        {
            try
            {
                var jobLang = _dataContext.JobVacancyLanguages.Where(v => v.IdjobVacancy == _idJob).DefaultIfEmpty();
                if (jobLang.Any()) {
                    foreach (var lang in jobLang)
                    {
                        _dataContext.Entry(lang).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    }
                }                
                var ret = _dataContext.SaveChanges();
                return ret > 0 ? true : false;
            }
            catch (Exception ex)
            {
                string message = $"Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}";
                _logger.LogError(message: message);
                return false;
            }
        }

        public IQueryable<JobVacancyLanguage> Get(int _idJob)
        {
            throw new NotImplementedException();
        }
    }
}
