namespace Domain.Repositories
{
    public interface ISalaryTypeRepository
    {
        public bool IsRightSalaryType(int salaryType);
        public bool IsRightSalaryValue(string salary, int salaryType);
    }
}
