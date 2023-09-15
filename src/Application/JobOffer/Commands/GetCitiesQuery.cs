using Application.Core;
using Domain.DTO.ManageJobs;
using MediatR;
using System.Runtime.Serialization;

namespace Application.JobOffer.Commands
{
    [DataContract]
    public class GetCitiesQuery : IRequest<Result<CitiesByOfferCompany>>
    {
        [DataMember]
        public int CompanyId { get; set; }
    }
}
