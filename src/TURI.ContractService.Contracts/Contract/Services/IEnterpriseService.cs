using Refit;

namespace TURI.ContractService.Contracts.Contract.Services
{
    public interface IEnterpriseService
    {

        [Get("/api/enterprise/GetAllActiveCompanies")]
        Task<List<int>> GetAllActiveCompanies();
    }
}
