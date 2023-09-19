using Application.Core;
using MediatR;
using System.Runtime.Serialization;

namespace Application.JobOffer.Commands
{
    [DataContract]
    public class VerifyOfferCommsCommand: IRequest<Result<bool>>
    {
        [DataMember]
        public int Offerid { get; set; }
    }
}
