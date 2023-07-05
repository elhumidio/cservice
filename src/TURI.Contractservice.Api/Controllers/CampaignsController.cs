using API.Controllers;
using Application.Aimwel.Commands;
using Application.Aimwel.Interfaces;
using Application.Aimwel.Queries;
using Domain.DTO;
using Domain.Entities;
using DPGRecruitmentCampaignClient;
using Microsoft.AspNetCore.Mvc;

namespace TURI.Contractservice.Controllers
{
    public class CampaignsController : BaseApiController
    {
        private readonly IAimwelCampaign _aimwelCampaign;

        public CampaignsController(IAimwelCampaign aimwelCampaign)
        {
            _aimwelCampaign = aimwelCampaign;
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
            var management = await Mediator.Send(new Application.JobOffer.Queries.CampaignsQueries.GetCampaignManagement.Query
            {
                IDJobVacancy = jobId
            });
            var request = new GetCampaignRequest { CampaignId = management.Value.ExternalCampaignId };
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
        public async Task<IActionResult> CancelCampaign(int jobId, int? modificationReason = 0)
        {
            try
            {
                var response = await Mediator.Send(new Cancel.Command { offerId = jobId, ModificationReason = modificationReason });
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

        [HttpGet()]
        public async Task<IActionResult> GetAimwelCampaignByAimwelId(string aimwelId)
        {
            try
            {
                var ret = await _aimwelCampaign.GetCampaignByAimwelId(aimwelId);
                return Ok(ret);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet("{jobId}")]
        public async Task<IActionResult> CreateCampaignUpdater(int jobId)
        {
            try
            {
                var response = await Mediator.Send(new CreateUpdater.Command
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


        [HttpPost]
        public async Task<IActionResult> VerifyOfferGoal(VerifyGoalsRequest request)
        {
            var response = await Mediator.Send(new VerifyOfferGoal.Verifier
            {
                OffersToVerify = request

            });

            return HandleResult(response);

        }
    }
}
