using Application.Core;
using Domain.Classes;
using MediatR;
using System.Runtime.Serialization;

namespace Application.Users.Commands
{
    [DataContract]
    public class GetUsersEmptyGetResponseCommand : IRequest<Result<List<UserGetResponseData>>>
    {

    }
}
