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

    }
}
