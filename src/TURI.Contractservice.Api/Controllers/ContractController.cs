using Application.Contracts.Queries;
using Application.EnterpriseContract.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ContractController : BaseApiController
    {
        /// <summary>
        /// Gets available units by contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("{contractId}")]
        public async Task<IActionResult> GetAvailableUnits(int contractId)
        {
            var result = await Mediator.Send(new GetAvailableUnits.Query
            {
                ContractId = contractId
            });
            return HandleResult(result);
        }

        /// <summary>
        /// Gets available units, given a contract and an owner (verifies consumed units and assignments)
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="Owner"></param>
        /// <returns></returns>
        [HttpGet("{contractId}/{Owner}")]
        public async Task<IActionResult> GetAvailableUnitsByOwner(int contractId, int Owner)
        {
            var result = await Mediator.Send(new GetAvailableUnitsByOwner.Query
            {
                ContractId = contractId,
                OwnerId = Owner
            });
            return HandleResult(result);
        }

        /// <summary>
        /// Gets assignments given a contract and an Owner
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="Owner"></param>
        /// <returns></returns>
        [HttpGet("{contractId}/{Owner}")]
        public async Task<IActionResult> GetAssignedUnitsByOwner(int contractId, int Owner)
        {
            var result = await Mediator.Send(new GetAssignedUnitsByOwner.Query
            {
                ContractId = contractId,
                OwnerId = Owner
            });
            return HandleResult(result);
        }

        [HttpGet("{companyId}/{type}/{regionid}")]
        public async Task<IActionResult> GetContract(int companyId, VacancyType type, int regionId)
        {
            var result = await Mediator.Send(new GetContract.Query
            {
                CompanyId = companyId,
                type = type,
                RegionId = regionId
            });
            return HandleResult(result);
           
           
        }
    }
}
