using Application.JobOffer.Commands;
using Application.JobOffer.DTO;
using Application.JobOffer.Queries;
using Application.Utils.Queries.Equest;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class JobOfferController : BaseApiController
    {


        /// <summary>
        /// Get active JobOffers 
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("{contractId}", Name = "GetActiveOffers")]
        public async Task<IActionResult> GetActiveOffers(int contractId)
        {
            var result = await Mediator.Send(new Application.JobOffer.Queries.ListActives.Query
            {
                ContractID = contractId,
            });
            return HandleResult(result);
        }


        /// <summary>
        /// File Ats Offer
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        [HttpPost(Name = "FileAtsOffer")]
        public async Task<IActionResult> FileAtsOffer(AtsOffer offer)
        {
            var result = await Mediator.Send(new FileAtsOffer.Command {
                   offer = offer   
            });
            var ret = HandleResult(result);
            return ret;
        }

        /// <summary>
        /// File Ats Offer
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        [HttpPost(Name = "FileOffers")]
        public async Task<IActionResult> FileOffers(List<int> _offers)
        {
            var result = await Mediator.Send(new FileJobs.Command
            {
                offers = _offers
            });
            var ret = HandleResult(result);
            return ret;
        }

        /// <summary>
        /// Publish an offer
        /// </summary>
        /// <param name="createOfferCommand"></param>
        /// <returns></returns>
        [HttpPost(Name = "Publish")]
        public async Task<IActionResult> PublishOffer(CreateOfferCommand createOfferCommand)
        {
            var result = await Mediator.Send(createOfferCommand);
            var ret = HandleResult(result);
            return ret;
        }

        /// <summary>
        /// Update offer
        /// </summary>
        /// <param name="createOfferCommand"></param>
        /// <returns></returns>
        [HttpPost(Name = "UpdateOffer")]
        public async Task<IActionResult> UpdateOffer(UpdateOfferCommand updateOfferCommand)
        {
            var result = await Mediator.Send(updateOfferCommand);
            var ret = HandleResult(result);
            return ret;
        }


        /// <summary>
        /// Get Consumed JobOffers Pack Checks (Don't verifies deleted, filed, finishdate, gets all published offers with a given contract) 
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("{contractId}", Name = "GetAllConsumedJobOffers")]
        public async Task<IActionResult> GetAllConsumedJobOffers(int contractId)
        {
            var result = await Mediator.Send(new Application.JobOffer.Queries.List.Query
            {
                ContractID = contractId,
            });
            return HandleResult(result);
        }


        /// <summary>
        /// Get Consumed JobOffers Pack or not pack by manager 
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("{contractId}/{ownerId}", Name = "GetConsumedJobOffersChecksByManager")]
        public async Task<IActionResult> GetConsumedJobOffersChecksByManager(int contractId, int ownerId)
        {
            var result = await Mediator.Send(new ListActivesByManager.Query
            {
                ContractID = contractId,
                OwnerID = ownerId

            });
            return HandleResult(result);
        }

        /// <summary>
        /// Get Consumed JobOffers Pack or not pack all managers By Contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("{contractId}", Name = "GetConsumedUnitsAutoFiltered")]
        public async Task<IActionResult> GetConsumedUnitsAutoFiltered(int contractId)
        {
            var result = await Mediator.Send(new ListAutoFiltered.Query
            {
                ContractID = contractId

            });
            return HandleResult(result);
        }

        /// <summary>
        /// Get consumed units welcome not Spain
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns> 
        [HttpGet("{companyId}", Name = "GetConsumedUnitsWelcomeNotSpain")]
        public async Task<IActionResult> GetConsumedUnitsWelcomeNotSpain(int companyId)
        {
            var result = await Mediator.Send(new GetConsumedUnitsWelcomeNotSpain.Query
            {
                CompanyId = companyId,

            });
            return HandleResult(result);
        }

        /// <summary>
        /// Get Consumed JobOffers Pack or not pack by company 
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns> 
        [HttpGet("{companyId}", Name = "GetActiveOffersByCompany")]
        public async Task<IActionResult> GetActiveOffersByCompany(int companyId)
        {
            var result = await Mediator.Send(new ListActivesByCompany.Query
            {
                CompanyId = companyId,

            });
            return HandleResult(result);
        }

        /// <summary>
        /// Gets Turijobs - Equest equivalent degree
        /// </summary>
        /// <param name="eqDegreeId"></param>
        /// <returns></returns>
        [HttpGet("{eqDegreeId}/{siteId}", Name = "GetEquestDegree")]
        public async Task<IActionResult> GetEquestDegree(int eqDegreeId, int siteId)
        {
            var result = await Mediator.Send(new DegreeEquivalent.Query
            {
                DegreeId = eqDegreeId,
                SiteId = siteId
            });
            return HandleResult(result);
        }

        /// <summary>
        /// Gets Turijobs - Equest equivalent industry code
        /// </summary>
        /// <param name="eqDegreeId"></param>
        /// <returns></returns>
        [HttpGet("{industryCodeId}", Name = "GeteQuestIndustryCode")]
        public async Task<IActionResult> GetEQuestIndustryCode(int industryCodeId)
        {
            var result = await Mediator.Send(new IndustryEquivalent.Query
            {
                industryCode = industryCodeId
            });
            return HandleResult(result);
        }

        /// <summary>
        /// Gets Turijobs - Equest equivalent country/region code
        /// </summary>
        /// <param name="eqDegreeId"></param>
        /// <returns></returns>
        [HttpGet("{countryStateId}", Name = "GetEQuestCountryState")]
        public async Task<IActionResult> GetEQuestCountryState(string countryStateId)
        {
            var result = await Mediator.Send(new CountryStateEQuivalent.Query
            {
                countryId = countryStateId
            });
            return HandleResult(result);
        }


        /// <summary>
        /// Gets idjobvacancy ats offer,  In case offer exists returns idjobvacancy else returns 0
        /// </summary>
        /// <param name="externalId"></param>
        /// <returns></returns>
        [HttpGet("{externalId}", Name = "GetAtsOffer")]
        public async Task<IActionResult> GetAtsOffer(string externalId)
        {
            var result = await Mediator.Send(new VerifyOffer.Query
            {
                ExternalId = externalId
            });
            return HandleResult(result);
        }



    }
}
