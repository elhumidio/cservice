using Application.Aimwel.Commands;
using Application.Aimwel.Interfaces;
using Application.Aimwel.Queries;
using Application.JobOffer.Commands;
using Application.JobOffer.DTO;
using Application.JobOffer.Queries;
using Application.Utils.Queries.Equest;
using Domain.Entities;
using DPGRecruitmentCampaignClient;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class JobOfferController : BaseApiController
    {
        private readonly IAimwelCampaign _aimwelCampaign;

        public JobOfferController(IAimwelCampaign aimwelCampaign)
        {
            _aimwelCampaign = aimwelCampaign;
        }

        /// <summary>
        /// Get active JobOffers
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("{contractId}")]
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
        /// Get Consumed JobOffers Pack or not pack by company
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> GetAllJobs()
        {
            var result = await Mediator.Send(new ListAllJobs.Query
            {
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

        /// <summary>
        /// Test purposes...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> TestAimwelCampaign()
        {
            var offer = new JobVacancy
            {
                IdzipCode = 7447,
                Idcountry = 40,
                Idregion = 33,
                Title = "TEST OFFER 3",
                Description = "DESCRIPTION TEST 3",
                IdjobVacancy = 223230,
                Idbrand = 2221,
                Identerprise = 2221,
                Idsite = 6
            };

            var ret = await _aimwelCampaign.CreateCampaing(offer);
            return Ok();
        }

        /// <summary>
        ///Test purposes...
        /// </summary>
        /// <param name="client"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<GetCampaignResponse> GetCampaign(int jobId)
        {
            var offer = Mediator.Send(new Get.Query
            {
                OfferId = jobId
            });
            var request = new GetCampaignRequest { CampaignId = offer.Result.AimwelCampaignId };
            var ans = await _aimwelCampaign.GetCampaign(request);
            return ans;
        }

        /// <summary>
        /// It Creates an Aimwel campaign
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet("{jobId}")]
        public async Task<IActionResult> CreateCampaign(int jobId)
        {
            try
            {
                var response = await Mediator.Send(new Create.Command
                {
                    offerId = jobId
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// It Cancel an Aimwel campaign
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet("{jobId}")]
        public async Task<IActionResult> CancelCampaign(int jobId)
        {
            try
            {
                var response = await Mediator.Send(new Cancel.Command { offerId = jobId });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// It Pauses an Aimwel campaign
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet("{jobId}")]
        public async Task<IActionResult> PauseCampaign(int jobId)
        {
            try
            {
                var response = await Mediator.Send(new Pause.Command { offerId = jobId });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// It resumes an Aimwel campaign
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet("{jobId}")]
        public async Task<IActionResult> ResumeCampaign(int jobId)
        {
            try
            {
                var response = await Mediator.Send(new Resume.Command { offerId = jobId });
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// It gets Aimwel campaign status
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetCampaignStatus(int jobId)
        {
            try
            {
                var response = await Mediator.Send(new GetStatus.Query
                {
                    OfferId = jobId
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
