using Domain.Enums;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public int GetUserIdByEmail(string email)
        {
            int userId = 0;
            var user = _dataContext.Users.FirstOrDefault(x => x.Email == email && (bool)x.ChkActive);
            if (user != null)
                userId = user.Idsuser;
            return userId;
        }

        public bool IsAdmin(int userId)
        {
            bool IsAdmin = false;
            var user = _dataContext.Users.Where(u => u.Idsuser == userId && u.ChkActive == true);
            if (user != null && user.Any())
            {
                IsAdmin = user.First().IdstypeUser == (int)UserTypes.AdministradorEmpresa;
            }
            return IsAdmin

        }
    }
}
