using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class UnitsRepository : IUnitsRepository
    {
        private readonly DataContext _dataContext;

        public UnitsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IQueryable<EnterpriseUserJobVac> GetAssignmentsByContract(int contractId)
        {
            var query = _dataContext.EnterpriseUserJobVacs.Where(a => a.Idcontract == contractId);
            return query;
        }



        IQueryable<EnterpriseUserJobVac> IUnitsRepository.GetAssignmentsByContractAndManager(int contractId, int manager)
        {
            var query = _dataContext.EnterpriseUserJobVacs.Where(a => a.Idcontract == contractId && a.IdenterpriseUser == manager);
            return query;
        }
    }
}
