using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUnitsRepository
    {
        public IQueryable<EnterpriseUserJobVac> GetAssignmentsByContract(int contractId);
        public IQueryable<EnterpriseUserJobVac> GetAssignmentsByContractAndManager(int contractId, int manager);
    }
}
