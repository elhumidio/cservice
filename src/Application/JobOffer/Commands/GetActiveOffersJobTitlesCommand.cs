using Application.Core;
using Application.JobOffer.DTO;
using MediatR;
using System.Runtime.Serialization;


namespace Application.JobOffer.Commands
{
    [DataContract]
    public class GetActiveOffersJobTitlesCommand : IRequest<Result<List<int?>>>
    {
    }
}
