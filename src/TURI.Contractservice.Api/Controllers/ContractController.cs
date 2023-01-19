using Application.Contracts.DTO;
using Application.Contracts.Queries;
using Application.EnterpriseContract.Queries;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

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
        /// It gets assignations by contract and owner
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetCompanyOwnersAssignmentsByContract(OwnersAssignmentsRequestDto info) {

            var result = await Mediator.Send(new GetUseOfUnitsByOwnerAndContract.Query {

                 ContractIds = info.ContractsList,
                 OwnerIds = info.OwnersList
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



        /// <summary>
        /// Gets assigned units Portugal or Mexico by Company
        /// </summary>
        /// <param name="companyId"></param>        
        /// <returns></returns>
        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetAvailableUnitsMexicoOrPortugal(int companyId)
        {
            var result = await Mediator.Send(new GetAvailableUnitsMexicoOrPortugal.Query
            {
               CompanyId = companyId
            });
            return HandleResult(result);
        }

        /// <summary>
        /// Gets assignments given a contract and an Owner
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetAvailableUnitsByCompany(int companyId)
        {
            var result = await Mediator.Send(new GetUnitsByCompany.Query
            {
                CompanyId = companyId
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

        /// <summary>
        /// Get users that contrats expire soon
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        [HttpGet("{days}")]
        public async Task<IActionResult> GetUsersContractExpireSoon(int days)
        {
            var result = await Mediator.Send(new GetUsersContractExpireSoon.Query
            {
                Days = days
            });
            return HandleResult(result);
        }

        /// <summary>
        /// Get users that contrats expire soon
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetUsersContractAvailableUnits()
        {
            var result = await Mediator.Send(new GetUsersContractAvailableUnits.Query
            {
                
            });
            return HandleResult(result);
        }

        /// <summary>
        /// Get users that contrat begin today
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetUsersContractBegin()
        {
            var result = await Mediator.Send(new GetUsersContractBegin.Query
            {
                
            });
            return HandleResult(result);
        }
    }
}
