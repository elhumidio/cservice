using Application.Core;
using Domain.DTO;
using MediatR;
using System.Runtime.Serialization;

namespace Application.JobTitles.Command
{
    [DataContract]
    public class GetJobTitlesDenominationsFromActiveOffersCommand : IRequest<Result<JobTitleDenominationsDto[]>>
    {
        [DataMember]
        public int LanguageId { get; set; }
    }
}
