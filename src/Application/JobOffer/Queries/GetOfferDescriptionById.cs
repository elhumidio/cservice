using Application.Core;
using Application.JobOffer.DTO;
using Application.Utils;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TURI.ApplicationService.Contracts.Application.Services;
using TURI.EnterpriseService.Contracts.Models;
using TURI.EnterpriseService.Contracts.Services;
using TURI.SearchService.Contracts.Search.Services;

namespace Application.JobOffer.Queries
{
    public class GetOfferDescriptionById
    {
        public class Query : IRequest<Result<OfferDescriptionDTO>>
        {
            public int OfferId { get; set; }
            public int LanguageId { get; set; }
            public int SiteId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<OfferDescriptionDTO>>
        {
            private readonly IJobOfferRepository _jobOffer;
            private readonly ICountryRepository _countryOffer;
            private readonly IRegionRepository _regionOffer;
            private readonly IEnterpriseRepository _enterpriseOffer;
            private readonly IDegreeRepository _degreeOffer;
            private readonly IJobExpYearsRepository _jobExpYearsOffer;
            private readonly IJobCategoryRepository _jobCategoryOffer;
            private readonly IResidenceTypeRepository _residenceTypeOffer;
            private readonly ISalaryTypeRepository _salaryTypeOffer;
            private readonly ICurrencyRepository _currencyRepository;
            private readonly IFieldRepository _fieldRepository;
            private readonly IMapper _mapper;
            private readonly IConfiguration _config;
            private readonly IApplicationService _applicationService;
            private readonly IEnterpriseService _enterpriseService;
            private readonly ISearchService _searchService;

            public Handler(IMapper mapper, IConfiguration config, IJobOfferRepository jobOffer, ICountryRepository countryOffer, IRegionRepository regionOffer, IEnterpriseRepository enterpriseOffer, IApplicationService applicationService, IEnterpriseService enterpriseService, IDegreeRepository degreeOffer, IJobExpYearsRepository jobExpYearsOffer, IJobCategoryRepository jobCategoryOffer, IResidenceTypeRepository residenceTypeOffer, ISalaryTypeRepository salaryTypeOffer, ICurrencyRepository currencyRepository, IFieldRepository fieldRepository, ISearchService searchService)
            {
                _mapper = mapper;
                _config = config;
                _jobOffer = jobOffer;
                _countryOffer = countryOffer;
                _regionOffer = regionOffer;
                _enterpriseOffer = enterpriseOffer;
                _applicationService = applicationService;
                _enterpriseService = enterpriseService;
                _degreeOffer = degreeOffer;
                _jobExpYearsOffer = jobExpYearsOffer;
                _jobCategoryOffer = jobCategoryOffer;
                _residenceTypeOffer = residenceTypeOffer;
                _salaryTypeOffer = salaryTypeOffer;
                _currencyRepository = currencyRepository;
                _fieldRepository = fieldRepository;
                _searchService = searchService;
            }

            public async Task<Result<OfferDescriptionDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var offer = _jobOffer.GetOfferById(request.OfferId);
                if (offer == null)
                {
                    return Result<OfferDescriptionDTO>.Failure($"Error trying to get offer description {request.OfferId}.");
                }
                var enterprise = _enterpriseOffer.Get(offer.Identerprise);
                if (enterprise == null)
                {
                    return Result<OfferDescriptionDTO>.Failure($"Error trying to get enterprise description {request.OfferId}.");
                }
                // Offer data.
                OfferDescriptionDTO offerDescription = new OfferDescriptionDTO();
                offerDescription.OfferId = request.OfferId;
                offerDescription.Title = offer.Title;
                offerDescription.JobLocation = GetJobLocation(offer.Idcountry, offer.Idregion, request.LanguageId, offer.JobLocation ?? string.Empty);
                offerDescription.Description = offer.Description;
                offerDescription.PublicationDate = offer.PublicationDate;
                string logoURL = _enterpriseOffer.GetCompanyLogo(offer.Identerprise, offer.Idbrand, offer.ChkBlindVac);
                offerDescription.Logo = GetEnterpriseLogoURL(logoURL, offer.Idsite);
                offerDescription.Vacancies = offer.VacancyNumber;
                offerDescription.TotalCandidateApplied = await GetApplicationCountByOffer(offer.IdjobVacancy);
                // Salary.
                if (!offer.ChkBlindSalary.GetValueOrDefault())
                { 
                    offerDescription.SalaryMin = offer.SalaryMin;
                    offerDescription.SalaryMax = offer.SalaryMax;
                    if (offer.SalaryCurrency.HasValue)
                    { 
                        offerDescription.SalaryCurrency = _currencyRepository.GetCurrencyById(offer.SalaryCurrency.Value, request.SiteId, request.LanguageId);
                    }
                    var salaryType = _salaryTypeOffer.GetSalaryTypeById(offer.IdsalaryType, request.SiteId, request.LanguageId);
                    if (salaryType != null)
                    {
                        offerDescription.SalaryType = salaryType.BaseName;
                    }
                }
                offerDescription.Tags = await GetOfferTags(offer.IdjobVacancy, request.LanguageId, request.SiteId);
                offerDescription.Tasks = await GetOfferTasks(offer.IdjobVacancy, request.LanguageId);
                offerDescription.Skills = await GetOfferSkills(offer.IdjobVacancy, request.LanguageId);
                offerDescription.Benefits = await GetOfferBenefits(offer.IdjobVacancy, request.LanguageId);
                offerDescription.Preferences = await GetOfferPreferences(offer.IdjobVacancy, request.LanguageId);
                // Requirements.
                offerDescription.Requirements = new List<string>();
                var degree = _degreeOffer.GetDegreeById(offer.Iddegree, request.SiteId, request.LanguageId);
                if (degree != null)
                {
                    offerDescription.Requirements.Add(degree.BaseName);
                }
                var jobExpYear = _jobExpYearsOffer.GetJobExperienceYearsById(offer.IdjobExpYears, request.SiteId, request.LanguageId);
                if (jobExpYear != null)
                {
                    offerDescription.Requirements.Add(jobExpYear.BaseName);
                }
                if (offer.IdjobCategory.GetValueOrDefault() > 0)
                {
                    var jobCategory = _jobCategoryOffer.GetJobCategoryById(offer.IdjobCategory.GetValueOrDefault(), request.SiteId, request.LanguageId);
                    if (jobCategory != null)
                    {
                        offerDescription.Requirements.Add(jobCategory.BaseName);
                    }
                }
                var residenceType = _residenceTypeOffer.GetResidenceTypeById(offer.IdresidenceType, request.SiteId, request.LanguageId);
                if (residenceType != null)
                {
                    offerDescription.Requirements.Add(residenceType.BaseName);
                }
                if (!string.IsNullOrEmpty(offer.Requirements))
                {
                    offerDescription.Requirements.Add(offer.Requirements);
                }
                // Enterprise data.
                offerDescription.CompanyId = enterprise.Identerprise;
                offerDescription.CompanyName = _enterpriseOffer.GetCompanyNameCheckingBlind(offer.Identerprise, offer.ChkBlindVac);
                offerDescription.CompanyLocation = GetLocation(enterprise.Idcountry, enterprise.Idregion, request.LanguageId); // TODO
                offerDescription.CompanyDescription = _enterpriseOffer.GetCompanyDescriptionCheckingBlind(offer.Identerprise, offer.ChkBlindVac);
                offerDescription.Employees = enterprise.Employees;
                offerDescription.CompanyField = _fieldRepository.GetFieldById(enterprise.Idfield, request.SiteId, request.LanguageId);
                offerDescription.CompanyURL = enterprise.UrlWeb ?? string.Empty;
                return Result<OfferDescriptionDTO>.Success(offerDescription);
            }

            private string GetLocation(int countryId, int regionId, int languageId)
            {
                string countryName = string.Empty;
                var country = _countryOffer.GetCountryById(countryId);
                if (country != null)
                {
                    countryName = country.BaseName;
                }
                string regionName = string.Empty;
                var region = _regionOffer.GetRegionNameByID(regionId, languageId == 17);
                if (region != null)
                {
                    regionName = region;
                }
                return $"{regionName}, {countryName}";
            }

            private string GetJobLocation(int countryId, int regionId, int languageId, string jobLocation)
            {
                string location = GetLocation(countryId, regionId, languageId);
                if (string.IsNullOrEmpty(jobLocation))
                {
                    return location;
                }
                return $"{jobLocation}, {location}";
            }

            private string GetEnterpriseLogoURL(string logoURL, int siteId)
            {
                if (logoURL.Contains("static"))
                {
                    return logoURL;
                }
                return $"{_config["Aimwel:Portal.urlRootStatics"]}" +
                       $"{"/img/"}" +
                       $"{ApiUtils.GetShortCountryBySite((Sites)siteId)}" +
                       $"{"/logos/"}" +
                       $"{logoURL}";
            }

            private async Task<int> GetApplicationCountByOffer(int offerId)
            {
                var response = await _applicationService.CountApplicantsByOffers(null, new int[] { offerId });
                if (response == null || response.Length == 0)
                {
                    return 0;
                }
                return response[0].ApplicationsCount;
            }

            private async Task<List<string>> GetOfferTags(int offerId, int languageId, int siteId)
            {
                var tags = await _searchService.GetOfferTags(new TURI.SearchService.Contracts.Search.Models.SearchOfferTagsModel()
                {
                    OfferId = offerId,
                    LanguageId = languageId,
                    SiteId = siteId
                });
                if (tags == null || tags.Count == 0)
                {
                    return new List<string>();
                }
                return tags;
            }

            private async Task<List<string>> GetOfferTasks(int offerId, int languageId)
            {
                var tasks = await _enterpriseService.GetOfferTasks(offerId);
                if (tasks == null || tasks.Count == 0)
                {
                    return new List<string>();
                }
                List<string> response = new List<string>();
                foreach (var task in tasks)
                {
                    response.Add(task.Translations.TextName);
                }
                return response;
            }

            private async Task<List<string>> GetOfferSkills(int offerId, int languageId)
            {
                var skills = await _enterpriseService.GetOfferSkills(offerId);
                if (skills == null || skills.Count == 0)
                {
                    return new List<string>();
                }
                List<string> response = new List<string>();
                foreach (var skill in skills)
                {
                    response.Add(skill.Translations.TextName);
                }
                return response;
            }

            private async Task<List<string>> GetOfferPreferences(int offerId, int languageId)
            {
                var preferences = await _enterpriseService.GetOfferPreferences(offerId);
                if (preferences == null || preferences.Count == 0)
                {
                    return new List<string>();
                }
                List<string> response = new List<string>();
                foreach (var preference in preferences)
                {
                    response.Add(preference.Translations.TextName);
                }
                return response;
            }

            private async Task<List<string>> GetOfferBenefits(int offerId, int languageId)
            {
                var benefits = await _enterpriseService.GetOfferBenefits(offerId);
                if (benefits == null || benefits.Count == 0)
                {
                    return new List<string>();
                }
                List<string> response = new List<string>();
                foreach (var benefit in benefits)
                {
                    response.Add(benefit.Translations.TextName);
                }
                return response;
            }
        }
    }
}
