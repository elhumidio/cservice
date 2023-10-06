using Refit;
using TURI.ContractService.Contract.Models;
using TURI.ContractService.Contracts.Contract.Models;
using TURI.ContractService.Contracts.Contract.Models.ManageJobs;
using TURI.ContractService.Contracts.Contract.Models.Requests;
using TURI.ContractService.Contracts.Contract.Models.Response;

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

    [Post("/api/JobOffer/ListOffersAtsInfo")]
    Task<List<OfferMinInfoAts>> ListOffersAtsInfo(OfferMinInfoAtsRequest request);

    [Get("/api/JobOffer/GetActiveJobOffersByCompanyForFeeds/{companyId}")]
    Task<List<JobOfferResponse>> GetActiveJobOffersByCompanyForFeeds(int companyId);

    [Post("/api/JobOffer/GetActiveOffersForEmployerByCompaniesIds")]
    Task<KeyValuesResponse[]> GetActiveOffersForEmployerByCompaniesIds(ListCompaniesIdsRequest request);

    [Post("/api/JobOffer/GetCompaniesOffersPerDay")]
    Task<CompanyOffersPerDayResponse[]> GetCompaniesOffersPerDay(DateTimeRequest request);
}
