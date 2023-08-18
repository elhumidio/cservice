using Domain.DTO.Requests;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IContractProductRepository
    {
        public bool IsPack(int contractId);

        public int GetIdProductByContract(int contractId);
        public Task<int> CreateContractProduct(ContractProduct contract);
        public Task<bool> UpdateContractProductSalesforceId(UpdateContractProductSForceId obj);
    }
}
