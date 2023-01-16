using Application.Core;
using Domain.Classes;
using Domain.Repositories;
using MediatR;

namespace Application.EnterpriseContract.Queries
{
    public class GetUsersContractBegin
    {
        public class Query : IRequest<Result<List<UserContractBegin>>>
        {

        }

        public class Handler : IRequestHandler<Query, Result<List<UserContractBegin>>>
        {
            private readonly IUserRepository _user;

            public Handler(IUserRepository user)
            {
                _user = user;
            }

            public async Task<Result<List<UserContractBegin>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<UserContractBegin>>.Success(await _user.GetUsersContractBegin());
            }
        }
    }
}
