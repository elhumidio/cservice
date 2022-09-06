using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class SalaryRepository : ISalaryRepository
    {
        private DataContext _dataContext;

        public SalaryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<Salary> GetSalaries(int siteId, int languageId)
        {
            var salaries = _dataContext.Salaries
                .Where(a => a.Idsite == siteId && a.Idslanguage == languageId)
                .Where(a => a.ChkActive == 1);

            if (salaries != null)
            {
                return salaries;
            }
            else
            {
                return null;
            }
        }
    }
}
