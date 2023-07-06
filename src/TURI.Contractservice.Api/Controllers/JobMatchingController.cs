using API.Controllers;
using Application.JobVacMatching.Commands;
using Microsoft.AspNetCore.Mvc;

namespace TURI.Contractservice.Controllers
{
    public class JobMatchingController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetExternalJobId(int jobId)
        {
            var response = await Mediator.Send(new AtsJobIdCommand() {JobVacancyID = jobId } );
            return HandleResult(response);
        }
    }
}
