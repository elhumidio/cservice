using Application.Core;
using Domain.DTO.ManageJobs;
using MediatR;
using System.Runtime.Serialization;

namespace Application.JobOffer.Commands
{
    [DataContract]
    public class GetBrandsQuery : IRequest<Result<BrandsByOfferCompany>>
    {
        [DataMember]
        public int CompanyId { get; set; }
    }
}
