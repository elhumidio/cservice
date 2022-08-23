using Domain.Entities;

namespace Domain.Repositories
{
    public interface IJobVacTypeRepository
    {

        public IQueryable<JobVacType> GetJobVacTypes(int siteId, int languageId);
    }
}
