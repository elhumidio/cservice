using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class EnterpriseUserJobVacRepository : IEnterpriseUserJobVacRepository
    {
        private readonly DataContext _dataContext;

        public EnterpriseUserJobVacRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<int> Add(EnterpriseUserJobVac ujobvac)
        {
            var ret = await _dataContext.AddAsync(ujobvac);
            return ret.Entity.Idcontract;
        }

        public async Task<List<EnterpriseUserJobVac>> GetAssignmentsByUserProductAndContract(int idEnterpriseUser, int idjobvactype, int idcontract)
        {
            var dist = await _dataContext.EnterpriseUserJobVacs.Where(x => x.IdjobVacType == idjobvactype
            && x.Idcontract == idcontract && x.IdenterpriseUser == idEnterpriseUser).ToListAsync();
            return dist;
            
        }

        public async Task<bool> UpdateUnitsAssigned(EnterpriseUserJobVac jvac)
        {
            var ret = _dataContext.Update(jvac);
            var ent = await _dataContext.SaveChangesAsync();
            return ent > 0;
        }

        public async Task<List<EnterpriseUserJobVac>> GetAssignmentsByUserIDProductAndContract(int idEnterpriseUser, int idprod, int idcontract)
        {
            var dist = await _dataContext.EnterpriseUserJobVacs.Where(x => x.Idproduct == idprod
            && x.Idcontract == idcontract && x.IdenterpriseUser == idEnterpriseUser).ToListAsync();
            return dist;

        }
    }
}
