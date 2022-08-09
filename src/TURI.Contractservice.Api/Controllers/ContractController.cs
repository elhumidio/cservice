﻿using Application.Contracts.Queries;
using Application.EnterpriseContract.Queries;
using Microsoft.AspNetCore.Mvc;
using Persistence.Enums;

namespace API.Controllers
{
    public class ContractController : BaseApiController
    {
        /// <summary>
        /// Gets available units by contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        [HttpGet("{contractId}", Name = "GetAvailableUnits")]
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
        [HttpGet("{contractId}/{Owner}", Name = "GetAvailableUnitsByOwner")]
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
        [HttpGet("{contractId}/{Owner}", Name = "GetAssignedUnitsByOwner")]
        public async Task<IActionResult> GetAssignedUnitsByOwner(int contractId, int Owner)
        {
            var result = await Mediator.Send(new GetAssignedUnitsByOwner.Query
            {
                ContractId = contractId,
                OwnerId = Owner
            });
            return HandleResult(result);
        }



        [HttpGet("{companyId}/{type}", Name = "contract")]
        public async Task<IActionResult> GetContract(int companyId, VacancyType type)
        {
            var result = await Mediator.Send(new GetContract.Query
            {
                CompanyId = companyId,
                type = type
            });
            return HandleResult(result);
        }









    }
}
