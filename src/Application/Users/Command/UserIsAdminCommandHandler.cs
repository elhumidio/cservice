using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.Users.Command
{
    public class UserIsAdminCommandHandler : IRequestHandler<UserIsAdminCommand, Result<bool>>
    {
        private IEnterpriseUserRepository _enterpriseUserRepository;
        private IUserRepository _userRepository;

        public UserIsAdminCommandHandler(IEnterpriseUserRepository enterpriseUserRepository,
            IUserRepository userRepository)
        {
            _enterpriseUserRepository = enterpriseUserRepository;
            _userRepository = userRepository;
        }

        public Task<Result<bool>> Handle(UserIsAdminCommand request, CancellationToken cancellationToken)
        {
            var admins = _enterpriseUserRepository.GetCompanyAdmins(request.EnterpriseId, request.AllowManagers);

            var isAdmin = admins.Any(ad => ad.UserId == request.EnterpriseUserId
                                    && (ad.IsAdmin || (request.AllowManagers && ad.IsManager)));

            return Task.FromResult(Result<bool>.Success(isAdmin));
            
        }
    }
}
