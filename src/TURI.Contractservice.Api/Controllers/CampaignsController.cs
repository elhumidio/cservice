using API.Controllers;
using Application.Aimwel.Queries;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace TURI.Contractservice.Controllers
{
    public class CampaignsController : BaseApiController
    {
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
