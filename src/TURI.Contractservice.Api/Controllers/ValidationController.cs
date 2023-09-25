using API.Controllers;
using Application.Contracts.Queries;
using Application.Users.Command;
using Microsoft.AspNetCore.Mvc;

namespace TURI.Contractservice.Controllers
{
    public class ValidationController : BaseApiController
    {

        /// <summary>
        /// Validates that the given contract has units
        /// </summary>
        /// <returns>Number of units available</returns>
        [HttpGet("{contractId}")]
        public async Task<IActionResult> UnitsAvailable(int contractId)
        {
            //Redirects to the ContractController method.
            var result = await Mediator.Send(new GetAvailableUnits.Query
            {
                ContractId = contractId
            });
            return HandleResult(result);
        }

        /// <summary>
        /// Confirms that the given user has Admin (IDSUserType = 4 for Admin, 5 for Manager)
        /// </summary>
        /// <param name="allowManagers">If True, checks for IDUser 4 AND 5, returns true for either</param>
        /// <returns>True if admin, false otherwise</returns>
        [HttpGet("{enterpriseUserId}/{enterpriseId}/{allowManagers}")]
        public async Task<IActionResult> UserIsAdmin(int enterpriseUserId, int enterpriseId, bool allowManagers = false)
        {
            var result = await Mediator.Send(new UserIsAdminCommand(enterpriseUserId, enterpriseId, allowManagers));
            
            return HandleResult(result);
        }
    }
}
