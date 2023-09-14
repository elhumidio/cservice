using Refit;
using TURI.ContractService.Contract.Models;
using TURI.ContractService.Contracts.Contract.Models.ManageJobs;
using TURI.ContractService.Contracts.Contract.Models.Requests;

namespace TURI.ContractService.Contracts.Contract.Services;

public interface IJobOfferService
{
    [Get("/api/JobOffer/GetActiveJobs")]
    Task<JobOfferResponse[]> GetActiveJobs(int maxActiveDays);

    [Get("/api/JobOffer/GetActiveJobsFollowedCompaniesSinceLastLogin")]
    Task<JobOfferResponse[]> GetActiveJobsFollowedCompaniesSinceLastLogin(string lastLoggin, string followedCompanies);

    [Post("/api/JobOffer/GetOffersForView")]
    Task<OfferInfoMinForViewResponse[]> GetOffersForView(OfferInfoRequest request);

    [Post("/api/JobOffer/GetOffersForDashBoard")]
    public Task<ManageJobsResponse> GetOffersForDashBoard(GetOffersForDashBoardRequest dto);
}
