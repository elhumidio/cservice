using Refit;
using TURI.ContractService.Contract.Models;
using TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder;
using TURI.ContractService.Contracts.Contract.Models.Requests;
using TURI.ContractService.Contracts.Contract.Models.Response;

namespace TURI.ContractService.Contracts.Contract.Services;

public interface IContractService
{
    [Get("/api/contract/GetAvailableUnits")]
    Task<AvailableUnitsResponse[]> GetAvailableUnits(int contractId);

    [Post("/api/JobOffer/GetEnterprisesByOffers")]
    Task<List<int>> GetEnterprisesByOffers(List<int> offerIds);

    [Post("/api/contract/CreateContract")]
    Task<ContractCreationResponse> CreateContract(ContractCreateRequest contract);

    [Post("/api/contract/UpdateContractSalesForceId")]
    Task<bool> UpdateContractSalesForceId(WrapperContractProductSalesforceIdRequest request);

    [Post("/api/contract/GetValidContractsByCompaniesIds")]
    Task<KeyValuesResponse[]> GetValidContractsByCompaniesIds(ListCompaniesIdsRequest request);

    [Post("/api/contract/GetCountAvailableUnitsByCompaniesIds")]
    Task<KeyValuesResponse[]> GetCountAvailableUnitsByCompaniesIds(ListCompaniesIdsRequest request);

    [Post("/api/contract/GetFinishDateContractClosingExpiringByCompaniesIds")]
    Task<KeyValuesDateTimeResponse[]> GetFinishDateContractClosingExpiringByCompaniesIds(ListCompaniesIdsRequest request);
}
