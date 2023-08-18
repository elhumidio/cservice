using Refit;
using TURI.ContractService.Contract.Models;
using TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder;
using TURI.ContractService.Contracts.Contract.Models.Requests;

namespace TURI.ContractService.Contracts.Contract.Services;

public interface IContractService
{
    [Get("/api/contract/GetAvailableUnits")]
    Task<AvailableUnitsResponse[]> GetAvailableUnits(int contractId);

    [Post("/api/JobOffer/GetEnterprisesByOffers")]
    Task<List<int>> GetEnterprisesByOffers(List<int> offerIds);

    [Post("/api/contract/CreateContract")]
    Task<ContractCreationResponse> CreateContract(ContractCreateRequest contract);

    Task<bool> UpdateContractSalesForceId(WrapperContractProductSalesforceIdRequest request);
}
