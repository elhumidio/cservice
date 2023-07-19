using API.Converters;
using Application.Aimwel.Interfaces;
using Application.JobOffer.Commands;
using Application.JobOffer.DTO;
using Application.JobOffer.Queries;
using Application.Utils.Queries.Equest;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TURI.ContractService.Contract.Models;

namespace API.Controllers
{
    public class JobOfferController : BaseApiController
    {
        private readonly IAimwelCampaign _aimwelCampaign;
        private readonly IMemoryCache _cache;

        public JobOfferController(IAimwelCampaign aimwelCampaign, IMemoryCache cache)
        {
            _aimwelCampaign = aimwelCampaign;
            _cache = cache;
        }

        /// <summary>
        /// Get active JobOffers
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("{contractId}")]
        public async Task<IActionResult> GetActiveOffers(int contractId)
        {
            var result = await Mediator.Send(new ListActives.Query
            {
                ContractID = contractId,
            });
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetFlowOfferById(GetFlowOfferByIdCommand command)
        {
            var result = await Mediator.Send(new GetFlowOfferById.Query
            {
                OfferId = command.OfferId,
                LanguageId = command.LanguageId
            });
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetOfferDescriptionById(GetOfferDescriptionByIdCommand command)
        {
            var result = await Mediator.Send(new GetOfferDescriptionById.Query
            {
                OfferId = command.OfferId,
                LanguageId = command.LanguageId,
                SiteId = command.SiteId
            });
            return HandleResult(result);
        }

        /// <summary>
        /// File Ats Offer
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FileAtsOffer(FileAtsOfferCommand offer)
        {
            var result = await Mediator.Send(offer);
            var ret = HandleResult(result);
            return ret;
        }

        /// <summary>
        /// File Offers (from Web)
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FileOffers(List<int> _ids)
        {
            var result = await Mediator.Send(new FileJobs.Command
            {

                id = _ids.First()
            });

            var ret = HandleResult(result);
            return ret;
        }


        /// <summary>
        /// Sends a command to file jobs for each item in the list of ids.
        /// </summary>
        /// <param name="_ids">List of ids to send the command to.</param>
        /// <returns>Returns an Ok result.</returns>
        [HttpPost]
        public async Task<IActionResult> FileOffersFeed(List<int> _ids)
        {
            foreach (var item in _ids)
            {
                await Mediator.Send(new FileJobs.Command
                {

                    id = item
                });
            }

            return Ok(new OfferModificationResult() { IsSuccess=true });
        }


        /// <summary>
        /// Delete Offers (from Web)
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeleteOffers(List<int> _ids)
        {
            var result = await Mediator.Send(new DeleteJobs.Command
            {
                id = _ids.First()
            });

            var ret = HandleResult(result);
            return ret;
        }

        /// <summary>
        /// It activates offer
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Activate(ActivateJobRequest request)
        {
            var result = await Mediator.Send(new Activate.Command
            {
                id = request.JobId,
                OwnerId = request.OwnerId
            });

            var ret = HandleResult(result);
            return ret;
        }

        /// <summary>
        /// File Offers (from Web)
        /// </summary>
        /// <param name="offer"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CloseOffers(JobClosingReasonDto _closingOffer)
        {
            var result = await Mediator.Send(new CloseJobs.Command
            {
                dto = _closingOffer
            });

            var ret = HandleResult(result);
            return ret;
        }

        /// <summary>
        /// Publish an offer
        /// </summary>
        /// <param name="createOfferCommand"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PublishOffer(CreateOfferCommand createOfferCommand)
        {
            try
            {
                var result = await Mediator.Send(createOfferCommand);
                var ret = HandleResult(result);
                return ret;
            }
            catch (Exception ex)
            {
                var ret = HandleResult(OfferModificationResult.Failure(new List<string> { ex.Message }));
                return ret;
            }
        }

        /// <summary>
        /// Update offer
        /// </summary>
        /// <param name="createOfferCommand"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateOffer(UpdateOfferCommand updateOfferCommand)
        {
            try
            {
                var result = await Mediator.Send(updateOfferCommand);
                var ret = HandleResult(result);
                return ret;
            }
            catch (Exception ex)
            {
                var ret = HandleResult(OfferModificationResult.Failure(new List<string> { ex.Message }));
                return ret;
            }
        }

        /// <summary>
        /// Get Consumed JobOffers Pack Checks (Don't verifies deleted, filed, finishdate, gets all published offers with a given contract)
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("{contractId}")]
        public async Task<IActionResult> GetAllConsumedJobOffers(int contractId)
        {
            var result = await Mediator.Send(new List.Query
            {
                ContractID = contractId,
            });
            return HandleResult(result);
        }


        /// <summary>
        /// It gets consumed units grouped by contract
        /// </summary>
        /// <param name="contractIds"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetConsumedUnitsGroupedByContracts(List<int> contractIds)
        {
            var result = await Mediator.Send(new ListAutoFilteredGroupByContracts.Query
            {
                ContractIDs = contractIds
            });
            return HandleResult(result);
        }

        /// <summary>
        /// Get Consumed JobOffers Pack or not pack by manager
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("{contractId}/{ownerId}")]
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
        /// Get Consumed JobOffers Pack or not pack by Company 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetConsumedJobOffersByCompany(ContractOwnerRequestDto dto)
        {
            var result = await Mediator.Send(new ListActivesByManagerList.Query
            {
                Dto = dto
            });
            return HandleResult(result);
        }


        /// <summary>
        /// Get Consumed JobOffers Pack or not pack all managers By Contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("{contractId}")]
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
        [HttpGet("{companyId}")]
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
        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetActiveOffersByCompany(int companyId)
        {
            var result = await Mediator.Send(new ListActivesByCompany.Query
            {
                CompanyId = companyId,
            });
            return HandleResult(result);
        }


        /// <summary>
        /// Gets active offers full data by company.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <returns>The active offers full data.</returns>
        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetActiveOffersFullDataByCompany(int companyId)
        {
            var result = await Mediator.Send(new ListActivesWholeDataByCompany.Query
            {
                CompanyId = companyId,
            });
            return HandleResult(result);
        }

        /// <summary>
        /// Get Active JobOffers
        /// </summary>
        /// <param name="maxActiveDays"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobOfferResponse[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActiveJobs(int maxActiveDays)
        {
                var result = await Mediator.Send(new ListActiveJobs.Query { MaxActiveDays = maxActiveDays });
                if (result.IsSuccess)
                {
                    var jobOffers = result.Value;
                    if (jobOffers == null)
                        return NotFound();

                    var response = jobOffers
                        .Select(jobOffer => jobOffer.ToModel())
                        .ToArray();

                    return Ok(response);
                }
                else
                {
                    return BadRequest(result.Error);
                }


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<int>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActiveOffersByIds(List<int>? offersIds)
        {
            var result = await Mediator.Send(new ListActiveJobsByIds.Query { OffersIds = offersIds });
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result.Error);
            }


        }

        /// <summary>
        /// Get Count JobOffers published since days
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        [HttpGet("{days}")]
        public async Task<IActionResult> CountOffersPublished(int days)
        {
            var result = await Mediator.Send(new CountOffersPublished.Query
            {
                Days = days,
            });
            return HandleResult(result);
        }

        /// <summary>
        /// Gets Turijobs - Equest equivalent degree
        /// </summary>
        /// <param name="eqDegreeId"></param>
        /// <returns></returns>
        [HttpGet("{eqDegreeId}/{siteId}")]
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
        [HttpGet("{industryCodeId}")]
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
        [HttpGet("{countryStateId}")]
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
        [HttpGet("{externalId}")]
        public async Task<IActionResult> GetAtsOffer(string externalId)
        {
            var result = await Mediator.Send(new VerifyOffer.Query
            {
                ExternalId = externalId
            });
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> ListOffersAtsInfo(OfferMinInfoAtsRequest dto)
        {
            var result = await Mediator.Send(new ListOffersAtsInfo.Get
            {
                 CompanyId = dto.CompanyId,
                 ExternalId = dto.ExternalId       

            });

            return HandleResult(result);    
        }



        /// <summary>
        /// It update job date
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet("{jobId}")]
        public async Task<IActionResult> UpdateDate(int jobId)
        {
            try
            {
                var response = await Mediator.Send(new UpdateDate.Command
                {
                    id = jobId

                });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


    }
}
