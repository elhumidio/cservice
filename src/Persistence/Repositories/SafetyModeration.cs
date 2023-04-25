using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class SafetyModeration : ISafetyModeration
    {
        private readonly DataContext _dataContext;

        public SafetyModeration(DataContext dataContext)
        {
            _dataContext = dataContext;            
        }
        public bool InsertModerations(OigSafety row)
        {
            try
            {
                var a = _dataContext.Add(row).Entity;
                var ret = _dataContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
            }
            return true;
        }
    }
}
