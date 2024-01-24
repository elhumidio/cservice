using Application.Core;
using MediatR;
using System.Runtime.Serialization;
using Domain.DTO;


namespace Application.JobOffer.Commands
{
    [DataContract]
    public class GetActiveOffersJobTitlesCommand : IRequest<Result<JobTitlesIdsDTO>>
    {
    }
}
