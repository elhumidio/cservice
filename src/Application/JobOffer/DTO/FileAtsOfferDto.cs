using Application.Core;
using MediatR;
using System.Runtime.Serialization;

namespace Application.JobOffer.DTO
{
    [DataContract]
    public class FileAtsOfferDto : IRequest<Result<Unit>>
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Application_email { get; set; }
        [DataMember]
        public string Application_url { get; set; }
        [DataMember]
        public string Application_reference { get; set; }
    }
}
