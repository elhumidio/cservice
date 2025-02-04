using Domain.DTO.Products;
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
        public List<ProductUnits> GetProductsAndUnitsByContract(int contractId);
        public List<ContractProduct> GetContractProducts(int contractId);
        public Task<bool> Update(ContractProduct contractProduct);
    }
}
