using Refit;
using TURI.ContractService.Contract.Models;

namespace TURI.ContractService.Contracts.Contract.Services;

public interface IContractService
{
    [Get("/api/contract/GetAvailableUnits")]
    Task<AvailableUnitsResponse[]> GetAvailableUnits(int contractId);
}
