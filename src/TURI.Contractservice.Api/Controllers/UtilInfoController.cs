using Amazon.Runtime.Internal.Util;
using Application.AuxiliaryData.DTO;
using Application.AuxiliaryData.Queries;
using Application.Core;
using Application.EnterpriseContract.Queries;
using Application.GetCompanyInfo.Queries;
using Application.Interfaces;
using Application.JobOffer.Queries;
using Application.Utils;
using Application.Utils.Queries;
using Domain.DTO;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace API.Controllers
{
    public class UtilInfoController : BaseApiController
    {

        private readonly IMemoryCache _cache;
        public IHostEnvironment _env;
        private readonly ISafetyModeration _moderation;
        private readonly IGeoNamesConector _geonames;

        public UtilInfoController(IHostEnvironment env, IMemoryCache memoryCache,ISafetyModeration moderation, IGeoNamesConector geoNamesConector)
        {
            _env = env;
            _cache = memoryCache;
            _moderation = moderation;
            _geonames = geoNamesConector;
        }

        [HttpGet("{countryIso}")]
        public async Task<IActionResult> GetCOuntryNameByIso(string countryIso)
        {
            var result = await Mediator.Send(new GetCountryNameByIso.Get
            {
                IsoCode = countryIso
            });
            return HandleResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetConfigValuesOther()
        {
            var result = Application.Core.Result<string>.Success(_env.EnvironmentName);
            return HandleResult(result);
        }

        [HttpGet("{email}", Name = "company")]
        public async Task<IActionResult> GetCompany(string email)
        {
            var result = await Mediator.Send(new GetCompanyInfo.Query
            {
                Email = email
            });
            return HandleResult(result);
        }

        [HttpGet("{companyId}", Name = "GetCompanyInfoById")]
        public async Task<IActionResult> GetCompanyByCompanyId(int companyId)
        {
            var result = await Mediator.Send(new GetCompanyInfoById.Query
            {
                CompanyId = companyId
            });
            return HandleResult(result);
        }

        [HttpGet]
        public IActionResult InsertIntoModerationTable()
        {
            using (var reader = new StreamReader("OIG_safety_v0.2.txt"))
            {
                List<string> listA = new List<string>();
                List<string> listB = new List<string>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var sentencesInLine = line.Split("||||");

                    foreach (var sentence in sentencesInLine)
                    {
                        var values = line.Split("|||");
                        var it = new OigSafety();
                        it.Column0 = values[0].Trim();
                        //if (!line.Contains(';'))
                        //    it.Column1 = "casual";
                        //else
                            it.Column1 = values[1].Trim();
                        _moderation.InsertModerations(it);
                    }
                }
            }

            return Ok();
        }

        [HttpGet("{cityName}")]
        public async Task<IActionResult> GetZipCodeFromCityName(string cityName)
        {
            var result = await Mediator.Send(new GetZipCodeFromCityName.Get(cityName));
            return HandleResult(result);
        }

        [HttpGet("{zipCode}/{countryId}")] 
        public async Task<IActionResult> GetIdsbyZipCodeAndCountry(string zipCode,int countryId)
        {
            var result = await Mediator.Send(new GetIdsbyZipCode.GetIds {
                 CountryId = countryId,
                  ZipCode = zipCode
            });
            return HandleResult(result);
        }


        [HttpGet("{siteId}/{languageId}", Name = "GetAreas")]
        public async Task<IActionResult> GetAreas(int siteId, int languageId)
        {
            var result = await Mediator.Send(new ListAreas.Query
            {
                languageID = languageId,
                siteID = siteId
            });
            return HandleResult(result);
        }

        [HttpGet("{siteId}/{languageId}", Name = "GetDegrees")]
        public async Task<IActionResult> GetDegrees(int siteId, int languageId)
        {
            var result = await Mediator.Send(new ListDegrees.Query
            {
                languageID = languageId,
                siteID = siteId
            });
            return HandleResult(result);
        }

        [HttpGet("{companyId}", Name = "GetBrands")]
        public async Task<IActionResult> GetBrands(int companyId)
        {
            var result = await Mediator.Send(new ListBrands.Query
            {
                companyID = companyId
            });
            return HandleResult(result);
        }

        [HttpGet("{siteId}/{languageId}", Name = "GetJobCategories")]
        public async Task<IActionResult> GetJobCategories(int siteId, int languageId)
        {
            var result = await Mediator.Send(new ListJobCategories.Query
            {
                languageID = languageId,
                siteID = siteId
            });
            return HandleResult(result);
        }

        [HttpGet("{siteId}/{languageId}", Name = "GetJobContractTypes")]
        public async Task<IActionResult> GetJobContractTypes(int siteId, int languageId)
        {
            var result = await Mediator.Send(new ListJobContractTypes.Query
            {
                languageID = languageId,
                siteID = siteId
            });
            return HandleResult(result);
        }

        [HttpGet("{siteId}/{languageId}", Name = "GetJobExpYears")]
        public async Task<IActionResult> GetJobExpYears(int siteId, int languageId)
        {
            var result = await Mediator.Send(new ListJobExpYears.Query
            {
                languageID = languageId,
                siteID = siteId
            });
            return HandleResult(result);
        }

        [HttpGet("{siteId}/{languageId}", Name = "GetCountries")]
        public async Task<IActionResult> GetCountries(int siteId, int languageId)
        {
            var result = await Mediator.Send(new ListCountries.Query
            {
                languageID = languageId,
                siteID = siteId
            });
            return HandleResult(result);
        }

        [HttpGet("{siteId}/{languageId}", Name = "GetRegions")]
        public async Task<IActionResult> GetRegions(int siteId, int languageId)
        {
            var result = await Mediator.Send(new ListRegions.Query
            {
                languageID = languageId,
                siteID = siteId
            });
            return HandleResult(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetDataFromZipCode(string zipcode, string countryIso)
        {
            var result = _geonames.GetPostalCodesCollection(zipcode, countryIso);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies() {

            List<BrandDTO> brands = new List<BrandDTO>();
            var cachedBrands = _cache.TryGetValue(CacheKeys.Brands, out brands);

            if (!cachedBrands)
            {
                var result = await Mediator.Send(new ListAllBrands.Query
                {

                });
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.NeverRemove
                };
                _cache.Set(CacheKeys.Brands, result.Value, cacheEntryOptions);
                return HandleResult(result);
            }
            else {

                return HandleResult(new Result<List<BrandDTO>>{ Value = brands, IsSuccess =true});                
            }           
            
        }

        [HttpGet]
        public async Task<IActionResult> GetTitles(int langId) {

            List<TitleLang> titles = new List<TitleLang>();
            CacheKeys.Titles = langId.ToString();
            var cachedTitles = _cache.TryGetValue(CacheKeys.Titles, out titles);

            if (!cachedTitles)
            {
                var result = await Mediator.Send(new ListTitles.Query
                {
                    LangId = langId

                });
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.High
                };
                _cache.Set(CacheKeys.Titles, result.Value, cacheEntryOptions);
                return HandleResult(result);

            }
            else {
                return HandleResult(new Result<List<TitleLang>> { Value = titles, IsSuccess = true });
            }
            
            

        }

        //[HttpGet("{siteId}/{languageId}", Name = "GetSalaries")]
        //public async Task<IActionResult> GetSalaries(int siteId, int languageId)
        //{
        //    var result = await Mediator.Send(new ListSalaries.Query
        //    {
        //        languageID = languageId,
        //        siteID = siteId
        //    });
        //    return HandleResult(result);
        //}

        [HttpGet("{siteId}/{languageId}", Name = "GetSalaryTypes")]
        public async Task<IActionResult> GetSalaryTypes(int siteId, int languageId)
        {
            var result = await Mediator.Send(new ListSalaryTypes.Query
            {
                languageID = languageId,
                siteID = siteId
            });
            return HandleResult(result);
        }

        [HttpGet("{siteId}/{languageId}", Name = "GetJobVacType")]
        public async Task<IActionResult> GetJobVacType(int siteId, int languageId)
        {
            var result = await Mediator.Send(new ListJobVacTypes.Query
            {
                languageID = languageId,
                siteID = siteId
            });
            return HandleResult(result);
        }

        [HttpGet("{siteId}/{languageId}", Name = "GetResidenceTypes")]
        public async Task<IActionResult> GetResidenceTypes(int siteId, int languageId)
        {
            var result = await Mediator.Send(new ListResidenceTypes.Query
            {
                languageID = languageId,
                siteID = siteId
            });
            return HandleResult(result);
        }

        [HttpGet("", Name = "GetSites")]
        public async Task<IActionResult> GetSites()
        {
            var result = await Mediator.Send(new ListSites.Query
            {
            });
            return HandleResult(result);
        }

        [HttpGet("{siteId}", Name = "GetLanguages")]
        public async Task<IActionResult> GetLanguages(int siteId)
        {
            var result = await Mediator.Send(new ListLanguages.Query
            {
                siteID = siteId
            });
            return HandleResult(result);
        }
    }
}
