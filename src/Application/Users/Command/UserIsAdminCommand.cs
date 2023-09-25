
using Application.Core;
using MediatR;

namespace Application.Users.Command
{
    public class UserIsAdminCommand : IRequest<Result<bool>>
    {
        public int EnterpriseUserId { get; set; }
        public int EnterpriseId { get; set; }
        public bool AllowManagers { get; set; }

        public UserIsAdminCommand(int userId, int enterpriseId, bool allowManagers = false)
        {
            EnterpriseUserId = userId;
            EnterpriseId = enterpriseId;
            AllowManagers = allowManagers;
        }
    }
}
