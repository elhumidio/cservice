using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class AimwelErrorsRepository : IAimwelErrorsRepository
    {
        private readonly DataContext _dataContext;

        public AimwelErrorsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        

        public void Add(AimwelCreationError err)
        {
            try
            {
                var a = _dataContext.Add(err).Entity;
                var ret = _dataContext.SaveChanges();
                
            }
            catch (Exception ex)
            {
                var a = ex;
                
            }
        }
    }
}
