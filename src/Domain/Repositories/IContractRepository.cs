using Domain.DTO;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IContractRepository
    {
        public IQueryable<Contract> Get(int contractId);

        public IQueryable<Contract> GetContracts(int companyId);

        bool IsValidContract(int contractId);

        public IQueryable<ServiceTypeDto> GetServiceTypes(int contractId);
        public Task<List<ContractsDistDto>> GetValidContracts(int companyId,int siteId, int langId);

        public Task<List<RegEnterpriseContract>> GetWithReg(int contractId);
    }
}
