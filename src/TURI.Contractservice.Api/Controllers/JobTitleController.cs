using API.Controllers;
using Application.Core;
using Application.JobTitles.Command;
using Application.JobTitles.Queries;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace TURI.Contractservice.Controllers
{
    public class JobTitleController : BaseApiController
    {
        [HttpGet]
        [Route("GetAllJobTitles")]
        public async Task<IActionResult> GetAllJobTitles()
        {
            var result = await Mediator.Send(new GetAllJobTitles.GetAll());

            return HandleResult(result);
        }

        //[HttpPost]
        //public async Task<IActionResult> GetAllJobTitlesDenominations(GetAllJobTitlesDenominationsCommand item)
        //{
        //    var result = await Mediator.Send(item);
        //    return HandleResult(result);
        //}

        [HttpPost]
        public async Task<IActionResult> GetAllJobTitlesDenominationsByLanguage(GetAllJobTitlesDenominationsByLanguageCommand item)
        {
            var result = await Mediator.Send(item);
            return HandleResult(result);
        }


        [HttpPost]
        public async Task<IActionResult> GetJobTitlesDenominationsFromActiveOffers(GetJobTitlesDenominationsFromActiveOffersCommand item)
        {
                var result = await Mediator.Send(item);
                return HandleResult(result);
        }
    }
}
