using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class RegJobVacWorkPermitRepository : IRegJobVacWorkPermitRepository
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<RegJobVacWorkPermitRepository> _logger;
        public RegJobVacWorkPermitRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<int> Add(RegJobVacWorkPermit permit)
        {
            try
            {
                var a = _dataContext.Add(permit).Entity;
                var ret = await _dataContext.SaveChangesAsync();
                return permit.IdjobVacancy;
            }
            catch (Exception ex)
            {
                string message = $"Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}";
                _logger.LogError(message: message);
                return -1;
            }
        }

        public async Task<int> Delete(int jobId)
        {
            var permits = _dataContext.RegJobVacWorkPermits.Where(p => p.IdjobVacancy == jobId);
            foreach (var permit in permits)
            {
                _dataContext.Set<RegJobVacWorkPermit>().Remove(permit);
            }
            var ret = await _dataContext.SaveChangesAsync();
            return ret;
        }
    }
}
