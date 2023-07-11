using Refit;

namespace TURI.ContractService.Contracts.Contract.Services
{
    public interface IJobVacService
    {
        [Get("/api/JobVac/GetExternalJobId")]
        Task<string> GetExternalJobId(int jobId);
    }
}
