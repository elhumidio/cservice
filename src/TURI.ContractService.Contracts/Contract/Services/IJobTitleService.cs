using Refit;
using TURI.ContractService.Contracts.Contract.Models.Requests;
using TURI.ContractService.Contracts.Contract.Models.Response;


namespace TURI.ContractService.Contracts.Contract.Services;

public interface IJobTitleService
{
    [Post("/api/JobTitle/GetAllJobTitlesDenominationsByLanguage")]
    Task<JobTitleDenominationsResponse[]> GetAllJobTitlesDenominationsByLanguage(GetAllJobTitlesDenominationsByLanguageRequest dto);
}
