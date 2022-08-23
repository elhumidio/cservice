using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class SalaryTypeRepository : ISalaryTypeRepository
    {
        DataContext _dataContext;
        public SalaryTypeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool IsRightSalaryType(int _salaryType)
        {
            var salaryTypes = _dataContext.SalaryTypes.Where(s => s.IdsalaryType == _salaryType);
            return salaryTypes.Any();
        }
        public bool IsRightSalaryValue(string _salary, int _salaryType)
        {
            var salary = _dataContext.Salaries.Where(s => s.Value == _salary && s.IdsalaryType == _salaryType);
            return salary.Any();
        }

        public IQueryable<SalaryType> GetSalaryTypes(int siteId, int languageId)
        {
            var salariesTypes = _dataContext.SalaryTypes
                .Where(a => a.Idsite == siteId && a.Idslanguage == languageId);

            if (salariesTypes != null)
            {
                return salariesTypes;
            }
            else
            {
                return null;
            }
        }

    }
}
