using Refit;

namespace TURI.ContractService.Contracts.Contract.Services
{
    internal interface IJobVacService
    {
        [Get("/api/JobMatchingController/GetExternalJobId")]
        Task<string> GetExternalJobId(int jobId);
    }
}
