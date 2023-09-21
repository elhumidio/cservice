using Application.Contracts.Queries;
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

        /// <summary>
        /// Gets all company IDs that are Visibly active - with an active, non-blind Job offer
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllActiveCompanies()
        {
            var result = await Mediator.Send(new GetAllActiveCompanies.GetAll());

            return HandleResult(result);
        }
    }
}
