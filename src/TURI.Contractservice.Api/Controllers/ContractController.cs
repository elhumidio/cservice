using API.Converters;
using Application.ContractCreation.Commands;
using Application.ContractCreation.Dto;
using Application.Contracts.DTO;
using Application.Contracts.Queries;
using Application.EnterpriseContract.Queries;
using Application.Managers.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using TURI.ContractService.Contract.Models;
using TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder;
using TURI.ContractService.Contracts.Contract.Models.Requests;

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

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AvailableUnitsResponse[]))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetAvailableUnits(int contractId)
        //{
        //    var query = new GetAvailableUnits.Get
        //    {
        //        ContractId = contractId
        //    };

        //    var result = await Mediator.Send(query);

        //    if (result.IsSuccess)
        //    {
        //        if (result.Value == null)
        //            return NotFound();
        //        else
        //        {
        //            var response = result.Value.Select(alert => alert.ToModel()).ToArray();
        //            return Ok(response);
        //        }
        //    }

        //    return BadRequest(result.Error);
        //}

        /// <summary>
        /// It gets assignations by contract and owner
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetCompanyOwnersAssignmentsByContract(OwnersAssignmentsRequestDto info)
        {
            var result = await Mediator.Send(new GetUseOfUnitsByOwnerAndContract.Query
            {
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

        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetValidContractsByCompany(int companyId)
        {
            var contracts = await Mediator.Send(new GetValidContractsByCompany.Get
            {
                CompanyId = companyId
            });
            return HandleResult(contracts);
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

        [HttpPost]
        public async Task<IActionResult> GetContractsAddedsByManager(List<int> managers)
        {
            var result = await Mediator.Send(new GetContractsAddedsByManagers.Get
            {
                Managers = managers
            });
            return HandleResult(result);
        }

        [HttpGet("{contractId}/{siteId}/{lang}")]
        public async Task<IActionResult> GetAllProductsbyContract(int contractId, int siteId, int lang)
        {
            var result = await Mediator.Send(new GetAllProductsByContract.GetProducts
            {
                ContractId = contractId,
                SiteId = siteId,
                LanguageID = lang
            });
            return HandleResult(result);
        }

        [HttpGet("{contractId}")]
        public async Task<IActionResult> GetRegionsAllowed(int contractId)
        {
            var result = await Mediator.Send(new GetRegionsAllowed.GetRegions
            {
                ContractId = contractId
            });
            return HandleResult(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContractCreationResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateContract(ContractCreateRequest contract)
        {
            try
            {
                var requestToModel = contract.ToDomain();
                var result = await Mediator.Send(requestToModel);
                var convertedResult = result.Value.ToCommand();
                return HandleResult(convertedResult);
            }
            catch (Exception ex)
            {                
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateContractSalesForceId(WrapperContractProductSalesforceIdRequest request)
        {
            try
            {
                var requestToModel = request.ToCommand();
                var result = await Mediator.Send(requestToModel);
                return HandleResult(result.IsSuccess);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
