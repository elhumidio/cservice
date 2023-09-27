using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class BackOfUserRepository : IBackOfUserRepository
    {
        private readonly DataContext _dataContext;

        public BackOfUserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int? GetUserBoID(int userId)
        {
            return _dataContext.BackOfficeUsers.FirstOrDefault(bo => bo.UserId == userId);
        }
    }
}
