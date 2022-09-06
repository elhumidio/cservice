using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class JobContractTypeRepository : IJobContractTypeRepository
    {
        private readonly DataContext _dataContext;

        public JobContractTypeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool IsRightContractType(int? _contractTypeId)
        {
            var contractType = _dataContext.JobContractTypes.Where(a => a.IdjobContractType == _contractTypeId);
            return contractType.Any();
        }

        public IQueryable<JobContractType> GetJobContractTypes(int siteId, int languageId)
        {
            var jobContractTypes = _dataContext.JobContractTypes
                .Where(a => a.Idsite == siteId && a.Idslanguage == languageId);

            if (jobContractTypes != null)
            {
                return jobContractTypes;
            }
            else
            {
                return null;
            }
        }
    }
}
