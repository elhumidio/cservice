using Application.Core;
using Application.DTO;
using Application.JobOffer.DTO;
using Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null ||
                (result.IsSuccess && result.Value == null))
                return NotFound();

            if (result.IsSuccess && result != null)
                return Ok(result.Value);

            return BadRequest(result.Error);
        }

        protected IActionResult HandleResult(OfferModificationResult result)
        {
            return Ok(result);
        }
        protected IActionResult HandleResult(ContractCreationResponse result)
        {
            return Ok(result);
        }
        protected IActionResult HandleResult(ContractResult result)
        {
            return Ok(result);
        }
        protected IActionResult HandleResult(bool result)
        {
            return Ok(result);
        }

        protected IActionResult HandleResult(VerifyGoalsOfferResponse result)
        {

            return Ok(result);


        }  

    }
}
