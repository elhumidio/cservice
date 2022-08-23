using Domain.Entities;

namespace Domain.Repositories
{
    public interface ISalaryRepository
    {
        public IQueryable<Salary> GetSalaries(int siteId, int languageId);
    }
}
