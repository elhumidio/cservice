using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IRegEnterpriseContractRepository
    {
        public Task<int> UpdateUnits(int contractId, int jobTypeId);

        public Task<int> IncrementAvailableUnits(int contractId, int jobTypeId);

        public Task<int> GetUnitsByType(int contractId, VacancyType type);
        public Task<int> Add(RegEnterpriseContract contract);
        public int GetUnitsByCreditType(int contractId, VacancyTypesCredits type);
    }
}
