using Amazon.Runtime.Internal.Util;
using Application.AuxiliaryData.DTO;
using Application.AuxiliaryData.Queries;
using Application.Core;
using Application.EnterpriseContract.Queries;
using Application.JobOffer.Queries;
using Application.Utils;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers
{
    public class UtilInfoController : BaseApiController
    {

        private readonly IMemoryCache _cache;           
        public IHostEnvironment _env;

        public UtilInfoController(IHostEnvironment env, IMemoryCache memoryCache)
        {
            _env = env;
            _cache = memoryCache; 
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
                    Priority = CacheItemPriority.High
                };
                _cache.Set(CacheKeys.Brands, result.Value, cacheEntryOptions);
                return HandleResult(result);
            }
            else {

                return HandleResult(new Result<List<BrandDTO>>{ Value = brands, IsSuccess =true});                
            }           
            
        }

        /*[HttpGet]
        public async Task<IActionResult> GetTitles(int langId) {



        }*/

        [HttpGet("{siteId}/{languageId}", Name = "GetSalaries")]
        public async Task<IActionResult> GetSalaries(int siteId, int languageId)
        {
            var result = await Mediator.Send(new ListSalaries.Query
            {
                languageID = languageId,
                siteID = siteId
            });
            return HandleResult(result);
        }

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
