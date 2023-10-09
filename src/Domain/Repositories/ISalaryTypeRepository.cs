using Domain.Entities;

namespace Domain.Repositories
{
    public interface ISalaryTypeRepository
    {
        public bool IsRightSalaryType(int salaryType);

        //public bool IsRightSalaryValue(string salary, int salaryType);

        public IQueryable<SalaryType> GetSalaryTypes(int siteId, int languageId);

        public SalaryType GetSalaryTypeById(int salaryTypeId, int siteId, int languageId);
    }
}
