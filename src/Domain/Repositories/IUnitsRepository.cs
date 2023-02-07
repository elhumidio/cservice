using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IUnitsRepository
    {
        public IQueryable<EnterpriseUserJobVac> GetAssignmentsByContract(int contractId);

        public IQueryable<EnterpriseUserJobVac> GetAssignmentsByContractAndManager(int contractId, int manager);

        public bool AssignUnitToManager(int contractId, VacancyType type, int ownerId);

        public bool TakeUnitFromManager(int contractId, VacancyType type, int ownerId);

        public int GetAssignedUnitsMxPtByCompany(int companyId);

        public Task<Dictionary<int, List<int>>> GetAssignedContractsForManagers(List<int> managers);
    }
}
