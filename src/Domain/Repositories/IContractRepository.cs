using Domain.DTO;
using Domain.DTO.Products;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IContractRepository
    {
        public IQueryable<Contract> Get(int contractId);

        public IQueryable<Contract> GetContracts(int companyId);

        bool IsValidContract(int contractId);

        public IQueryable<ServiceTypeDto> GetServiceTypes(int contractId);

        public Task<List<ContractsDistDto>> GetValidContracts(int companyId, int siteId, int langId);

        public Task<List<RegEnterpriseContract>> GetWithReg(int contractId);

        public Task<List<ContractProductShortDto>> GetAllProductsByContract(int contractId, int lang, int site);

        public Task<bool> IsAllowedContractForSeeingFilters(int contractId);

        public Task<IReadOnlyList<KeyValueResponse>> GetValidContractsByCompaniesIds(List<int> companiesIds);

        public Task<int> CreateContract(Contract contract);

        public Task<bool> UpdateContractSalesforceId(int contractId, string salesforceId);

        public Task<bool> UpdateContract(Contract contract);

        public bool DisableContract(int contractId);

        public DateTime GetContractFinishDate(List<ProductUnits> productUnits, int countryId = 40);

        public Task<IReadOnlyList<EnterpriseListContractsIdsDto>> GetContractsByCompaniesIds(List<int> companiesIds);

        public Task<IReadOnlyList<KeyValueDateTimeDto>> GetFinishDateContractClosingExpiringByCompaniesIds(List<int> companiesIds);

        public Task<Contract> GetContractByStripeSessionId(string stripeSessionId);

        public bool DeleteContractInfo(int contractId, bool IsContractPreserved);
        public Task<List<ContractsDistDto>> GetValidContractsByProduct(int companyId, int productId, int siteId, int langId);
    }
}
