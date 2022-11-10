using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ATSManagerAdminRepository : IATSManagerAdminRepository
    {
        private readonly DataContext _dataContext;
        private const int _global = 999;

        public ATSManagerAdminRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<AtsmanagerAdminRegion> Get(int companyId)
        {
            var list = new List<AtsmanagerAdminRegion>();   
            var ans = _dataContext.AtsmanagerAdminRegions.Where(c => c.CompanyId == companyId);
            if(ans != null)
                list = ans.ToList();
            return list;
        }
        public AtsmanagerAdminRegion GetGlobalOwner(int companyId)
        {   
            var globalManager = _dataContext.AtsmanagerAdminRegions.FirstOrDefault(m => m.CountryId == _global && m.RegionId == _global);
            if (globalManager != null)
                return globalManager;
            else return new AtsmanagerAdminRegion();
        }

    }
}
