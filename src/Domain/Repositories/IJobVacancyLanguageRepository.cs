using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IJobVacancyLanguageRepository
    {
        public IQueryable<JobVacancyLanguage> Get(int _idJob);
        public bool Delete(int _idJob);
        public bool Add(JobVacancyLanguage _lang);
    }
}
