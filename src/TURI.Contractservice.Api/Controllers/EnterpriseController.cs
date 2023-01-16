using Application.EnterpriseContract.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EnterpriseController : BaseApiController
    {

        /// <summary>
        /// Get Count Enterprises active
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> CountCompaniesActive()
        {
            var result = await Mediator.Send(new CountCompaniesActive.Query
            {

            });
            return HandleResult(result);
        }
    }
}
