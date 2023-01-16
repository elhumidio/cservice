using Application.Users.Commands;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController : BaseApiController
    {
        /// <summary>
        /// Get users with empty getResponseIds
        /// </summary>
        /// <returns></returns>

        [HttpGet()]
        public async Task<IActionResult> GetUsersEmptyGetResponse()
        {
            GetUsersEmptyGetResponseCommand candidateModel = new GetUsersEmptyGetResponseCommand();

            var result = await Mediator.Send(candidateModel);
            return HandleResult(result);
        }

        /// <summary>
        /// Update user getResponseId
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateUserGetResponse(UpdateUserGetResponseCommand user)
        {
            var result = await Mediator.Send(user);

            return HandleResult(result);
        }
    }
}
