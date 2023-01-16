using Application.Core;
using MediatR;
using System.Runtime.Serialization;

namespace Application.Users.Commands
{
    public class UpdateUserGetResponseCommand : IRequest<Result<bool>>
    {
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string GrcontactId { get; set; }
    }
}
