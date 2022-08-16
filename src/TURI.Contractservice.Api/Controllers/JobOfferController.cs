using Application.JobOffer.Commands;
using Application.JobOffer.DTO;
using Application.JobOffer.Queries;
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
        public async Task<IActionResult> FileAtsOffer(FileAtsOfferDto offer)
        {
            var result = await Mediator.Send(offer);
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
        /// Publish and update offer
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
    }
}
