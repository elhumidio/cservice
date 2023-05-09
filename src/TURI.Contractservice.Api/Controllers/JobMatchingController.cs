using API.Controllers;
using Application.JobVacMatching.Commands;
using Microsoft.AspNetCore.Mvc;

namespace TURI.Contractservice.Controllers
{
    public class JobMatchingController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> GetBizneoJobId(BizneoJobIdCommand command)
        {
            var response = await Mediator.Send(command);
            return HandleResult(response);
        }
    }
}
