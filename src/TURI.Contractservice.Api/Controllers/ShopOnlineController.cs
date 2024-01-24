using API.Controllers;
using Application.ContractProducts.Queries;
using Application.OnlineShop.Queries;
using Microsoft.AspNetCore.Mvc;

namespace TURI.Contractservice.Controllers
{
    public class ShopOnlineController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> GetProductsAndPricesAndDiscounts(GetProductsAndPricesAndDiscountsCommand cmd)
        {
            var result = await Mediator.Send(cmd);
            return HandleResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductsAndPricesAndDiscounts(int countryId)
        {
            var result = await Mediator.Send(new GetAllProductsPricesAndDiscounts.GetAll { CountryId = countryId});
            return HandleResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStripeProductIds()
        {
            var results = await Mediator.Send(new GetAllStripeProductIdsQuery());
            return HandleResult(results);
        }

        [HttpGet("{contractId}")]
        public async Task<IActionResult> GetProductFromContract(int contractId)
        {
            var result = await Mediator.Send(new GetProductFromContract.Query
            {
                ContractID = contractId
            });
            return HandleResult(result);
        }
    }
}
