using Domain.Repositories;

namespace Persistence.Repositories
{
    public class JobTypeRepository : IJobTypeRepository
    {
        DataContext _dataContext;


        public JobTypeRepository(DataContext dataContext)
        {

            _dataContext = dataContext;
        }
        public bool IsRightJobVacType(int _vacTypeId)
        {
            var type = _dataContext.JobVacTypes.Where(jvt => jvt.IdjobVacType == _vacTypeId);
            return type.Any();
        }
    }
}
