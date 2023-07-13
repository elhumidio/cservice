using Refit;
using TURI.ContractService.Contract.Models;

namespace TURI.ContractService.Contracts.Contract.Services;

public interface IJobOfferService
{
    [Get("/api/JobOffer/GetActiveJobs")]
    Task<JobOfferResponse[]> GetActiveJobs(int maxActiveDays);

    [Get("/api/JobOffer/GetActiveJobsFollowedCompaniesSinceLastLogin")]
    Task<JobOfferResponse[]> GetActiveJobsFollowedCompaniesSinceLastLogin(string lastLoggin, string followedCompanies);
    
    [Get("/api/JobOffer/GetOffersForView")]
    Task<OfferInfoMinForViewResponse[]> GetOffersForView(List<int> offerIds, int language, int site);
}
