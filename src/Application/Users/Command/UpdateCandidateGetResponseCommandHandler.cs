using Application.Core;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Users.Commands
{

    public class UpdateUserGetResponseCommandHandler : IRequestHandler<UpdateUserGetResponseCommand, Result<bool>>
    {
        private IUserRepository _userRepository;

        public UpdateUserGetResponseCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<bool>> Handle(UpdateUserGetResponseCommand request, CancellationToken cancellationToken)
        {
            User user = new User
            {
                Email = request.Email,
                GrcontactId = request.GrcontactId
            };

            var updateUserCandidate = _userRepository.UpdateUserGetResponse(user);
            if (!updateUserCandidate)
            {
                return Result<bool>.Failure("Failed to update get response candidate");
            }

            return Result<bool>.Success(true);
        }
    }
}

