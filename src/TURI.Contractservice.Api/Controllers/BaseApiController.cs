using Application.Core;
using Application.JobOffer.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            if (result == null ||
                (!result.IsSuccess && result.Value == null))
                return BadRequest(result);

            if (result.IsSuccess && result != null)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
