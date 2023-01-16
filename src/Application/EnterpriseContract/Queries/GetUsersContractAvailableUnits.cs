using Application.Core;
using Application.EnterpriseContract.DTO;
using Domain.Classes;
using Domain.Repositories;
using MediatR;

namespace Application.EnterpriseContract.Queries
{
    public class GetUsersContractAvailableUnits
    {
        public class Query : IRequest<Result<List<UserContractAvailableUnitsData>>>
        {

        }

        public class Handler : IRequestHandler<Query, Result<List<UserContractAvailableUnitsData>>>
        {
            private readonly IUserRepository _user;

            public Handler(IUserRepository user)
            {
                _user = user;
            }

            public async Task<Result<List<UserContractAvailableUnitsData>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<UserContractAvailableUnitsData>>.Success(await _user.GetUsersContractAvailableUnits());
            }
        }
    }
}
