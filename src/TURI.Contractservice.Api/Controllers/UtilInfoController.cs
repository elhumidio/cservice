using Application.JobOffer.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UtilInfoController : BaseApiController
    {

        [HttpGet("{email}", Name = "company")]
        public async Task<IActionResult> GetCompany(string email)
        {
            var result = await Mediator.Send(new GetCompanyInfo.Query
            {
                Email = email
            });
            return HandleResult(result);
        }


    }
}
