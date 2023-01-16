using Application.Core;
using Domain.Classes;
using Domain.Repositories;
using MediatR;

namespace Application.Users.Commands
{

    public class GetUsersEmptyGetResponseCommandHandler : IRequestHandler<GetUsersEmptyGetResponseCommand, Result<List<UserGetResponseData>>>
    {
        private IUserRepository _userRepository;
            
        public GetUsersEmptyGetResponseCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<List<UserGetResponseData>>> Handle(GetUsersEmptyGetResponseCommand request, CancellationToken cancellationToken)
        {
            return Result<List<UserGetResponseData>>.Success(await _userRepository.ListUsersEmptyGetResponse());
        }
    }
}

