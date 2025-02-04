using Application.JobOffer.DTO;
using MediatR;
using System.Runtime.Serialization;

namespace Application.JobOffer.Commands
{
    [DataContract]
    public class FileAtsOfferCommand : IRequest<OfferModificationResult>
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

        [DataMember]
        public string IDIntegration { get; set; }
    }
}
