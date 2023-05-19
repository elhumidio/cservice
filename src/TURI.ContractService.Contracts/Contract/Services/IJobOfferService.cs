using Refit;
using TURI.ContractService.Contract.Models;

namespace TURI.ContractService.Contracts.Contract.Services;

public interface IJobOfferService
{
    [Get("/api/JobOffer/GetActiveJobs")]
    Task<JobOfferResponse[]> GetActiveJobs(int maxActiveDays);
}
