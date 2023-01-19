using Application.Core;
using Application.EnterpriseContract.DTO;
using Domain.Classes;
using Domain.Repositories;
using MediatR;

namespace Application.EnterpriseContract.Queries
{
    public class GetUsersContractExpireSoon
    {
        public class Query : IRequest<Result<List<UserContractExpireSoonData>>>
        {
            public int Days { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UserContractExpireSoonData>>>
        {
            private readonly IUserRepository _user;

            public Handler(IUserRepository user)
            {
                _user = user;
            }

            public async Task<Result<List<UserContractExpireSoonData>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<UserContractExpireSoonData>>.Success(await _user.GetUsersContractExpireSoon(request.Days));
            }
        }
    }
}
