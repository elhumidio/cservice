using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IATSManagerAdminRepository
    {
        public List<AtsmanagerAdminRegion> Get(int companyId);
        public AtsmanagerAdminRegion GetGlobalOwner(int companyId);
    }
}
