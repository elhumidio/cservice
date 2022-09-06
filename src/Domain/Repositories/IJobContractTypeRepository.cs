using Domain.Entities;

namespace Domain.Repositories
{
    public interface IJobContractTypeRepository
    {
        public bool IsRightContractType(int? contractTypeId);

        public IQueryable<JobContractType> GetJobContractTypes(int siteId, int languageId);
    }
}
