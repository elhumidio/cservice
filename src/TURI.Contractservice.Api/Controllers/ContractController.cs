using API.Converters;
using Application.ContractCRUD.Commands;
using Application.ContractCRUD.Query;
using Application.ContractProducts;
using Application.ContractProducts.Commands;
using Application.ContractProducts.Queries;
using Application.Contracts.DTO;
using Application.Contracts.Queries;
using Application.EnterpriseContract.Queries;
using Application.Managers.Queries;
using Application.OnlineShop.Commands;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder;
using TURI.ContractService.Contracts.Contract.Models.Requests;
using TURI.ContractService.Contracts.Contract.Models.Response;
using UpdateContract = TURI.ContractService.Contracts.Contract.Models.Requests.UpdateContract;

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

        [HttpGet("{contractId}/{IsContractPreserved}")]
        public async Task<IActionResult> DeleteContractAndRelated(int contractId, bool IsContractPreserved)
        {
            var ret = await Mediator.Send(new DeleteContractAndRelated.Query
            {
                ContractID = contractId,
                HasToDeleteContract = IsContractPreserved
            });
            return HandleResult(ret);
        }


        [HttpGet("{contractId}")]
        public async Task<IActionResult> FinishPayment(int contractId)
        {
            var ret = await Mediator.Send(new FinishPayments.Query
            {
                ContractId = contractId
                
            });
            return HandleResult(ret);
        }

        [HttpPost]
        public async Task<IActionResult> AddDiscountToContractProduct(ProductDiscountRequest cmd)
        {
            var ret = await Mediator.Send(cmd);
            return HandleResult(ret);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContractAndPayment(UpdateContractPaymentCommand cmd)
        {
            var ret = await Mediator.Send(cmd);
            return HandleResult(ret);
        }

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


        [HttpGet("{sessionId}")]
        public async Task<IActionResult> GetContractByCheckoutSessionId(string sessionId)
        {
            var result = await Mediator.Send(new GetContractByCheckoutSessionId.Query
            {
                SessionId = sessionId
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

        [HttpGet("{companyId}/{type}/{regionId}")]
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

        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetUnitsDistributionByCompany(int companyId)
        {
            var dist = await Mediator.Send(new DistributionForCredits.Query { CompanyId = companyId });
            return HandleResult(dist);
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

        [HttpPost]
        public async Task<IActionResult> AddPayment(AddPaymentCommand cmd)
        {
            try
            {
                var result = await Mediator.Send(cmd);
                return HandleResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        [HttpPost]
        public async Task<IActionResult> UpdateSessionId(UpdateStripeSessionCommand cmd)
        {
            var result = await Mediator.Send(cmd);
            return HandleResult(result.Value);
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


        [HttpGet("{contractId}")]
        public async Task<IActionResult> GetContractFullInfo(int contractId)
        {
            var result = await Mediator.Send(new GetContractAndRelated.Query
            {
                ContractId = contractId
            });
            return HandleResult(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetWelcomeContractLost()
        {
            var result = await Mediator.Send(new GetWelcomeContractLost.Get{});
            var response = new ContractDueOfferInfoList
            {
                contractCreationResponses = result.Value
            };
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContractCreationResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateContract(ContractCreateRequest contract)
        {
            try
            {
                if (contract.CountryId < 1)
                {
                    return BadRequest("Country field is mandatory is mandatory");
                }
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
        public async Task<IActionResult> UpdateContract(UpdateContract contract)
        {
            try
            {
                var message = contract.ToDomain();
                var result = await Mediator.Send(message);
                return HandleResult(result.Value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteContract(int contractId)
        {
            try
            {
                var result = await Mediator.Send(new DeleteContractCommand() { IdContract = contractId });
                return HandleResult(result.Value);
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(KeyValuesResponse[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetValidContractsByCompaniesIds(ListCompaniesIdsRequest request)
        {
            var result = await Mediator.Send(new GetValidContractsByCompaniesIds.Get
            {
                CompaniesIds = request.CompaniesIds
            });

            if (result.IsSuccess)
            {
                var response = result.Value.Select(grData => grData.ToResponse()).ToArray();

                return Ok(response);
            }

            return BadRequest(result.Error);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(KeyValuesResponse[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCountAvailableUnitsByCompaniesIds(ListCompaniesIdsRequest request)
        {
            var result = await Mediator.Send(new GetCountAvailableUnitsByCompaniesIds.Get
            {
                CompaniesIds = request.CompaniesIds
            });

            if (result.IsSuccess)
            {
                var response = result.Value.Select(grData => grData.ToResponse()).ToArray();

                return Ok(response);
            }

            return BadRequest(result.Error);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(KeyValuesDateTimeResponse[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFinishDateContractClosingExpiringByCompaniesIds(ListCompaniesIdsRequest request)
        {
            var result = await Mediator.Send(new GetFinishDateContractClosingExpiringByCompaniesIds.Get
            {
                CompaniesIds = request.CompaniesIds
            });

            if (result.IsSuccess)
            {
                var response = result.Value.Select(grData => grData.ToResponse()).ToArray();

                return Ok(response);
            }

            return BadRequest(result.Error);
        }

        /// <summary>
        /// Selects a single contract with available units matching the Type. Chooses the one going to expire the soonest.
        /// </summary>
        /// <returns>ContractID</returns>
        [HttpGet]
        public async Task<IActionResult> GetContractForEnterprise(int enterpriseId, VacancyType contractType, int enterpriseUserId)
        {
            var result = await Mediator.Send(new GetContractForEnterprise.Query(contractType, enterpriseId, enterpriseUserId));

            return Ok(result);
        }

        [HttpGet("{contractId}")]
        public async Task<IActionResult> GetPaymentByContract(int contractId)
        {
            var result = await Mediator.Send(new ListPayments.Query
            {
                ContractId = contractId
            });
            return Ok(result.Value);
        }


        [HttpGet("{contractId}")]
        public async Task<IActionResult> GetPaymentsByContract(int contractId)
        {
            var result = await Mediator.Send(new ListAllPayments.Query
            {
                ContractId = contractId
            });
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> GetCreditsInfoAvailableByCompanyAndUser(AvailableCreditsByTypeCompanyAndUserRequest cmd)
        {
            var result = await Mediator.Send(cmd);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetCreditsInfoAvailableByCompanyAndUsers(AvailableCreditsByTypeCompanyAndUsersRequest cmd)
        {
            var result = await Mediator.Send(cmd);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubtractUnitsManager(AddSubtractCreditsManagerCommand cmd)
        {
            var result = await Mediator.Send(cmd);
            return HandleResult(result);
        }

        [HttpGet("{enterpriseId}")]
        public async Task<IActionResult> GetCreditsPerProduct(int enterpriseId)
        {
            var result = await Mediator.Send(new GetCreditsPerProduct.Query() { EnterpriseId = enterpriseId });
            return HandleResult(result);
        }

    }
}
