using Domain.DTO.Distribution;
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

        public async Task<List<EnterpriseUserJobVacDto>> GetAssignmentsByUserProductAndContractForOffers(int idEnterpriseUser, int idjobvactype, int idcontract)
        {


            var distribution = await (from vacs in _dataContext.EnterpriseUserJobVacs
                               join contracts in _dataContext.Contracts on vacs.Idcontract equals contracts.Idcontract
                               where vacs.IdjobVacType == idjobvactype && vacs.Idcontract == idcontract && vacs.IdenterpriseUser == idEnterpriseUser
                               select new EnterpriseUserJobVacDto
                               {
                                   Idcontract = vacs.Idcontract,
                                   IdenterpriseUser = vacs.Idcontract,
                                   IdjobVacType = vacs.IdjobVacType,
                                   Idproduct = vacs.Idproduct,
                                   JobVacUsed = vacs.JobVacUsed,
                                   MaxJobVacancies = vacs.MaxJobVacancies,
                                   StartDate = contracts.StartDate ?? DateTime.Now

                               }).ToListAsync();
            return distribution;            
        }

        public async Task<List<EnterpriseUserJobVac>> GetAssignmentsByUserProductAndContract(int idEnterpriseUser, int idjobvactype, int idcontract)
        {


            var distribution = await (from vacs in _dataContext.EnterpriseUserJobVacs
                                      join contracts in _dataContext.Contracts on vacs.Idcontract equals contracts.Idcontract
                                      where vacs.IdjobVacType == idjobvactype && vacs.Idcontract == idcontract && vacs.IdenterpriseUser == idEnterpriseUser
                                      select new EnterpriseUserJobVac
                                      {
                                          Idcontract = vacs.Idcontract,
                                          IdenterpriseUser = vacs.Idcontract,
                                          IdjobVacType = vacs.IdjobVacType,
                                          Idproduct = vacs.Idproduct,
                                          JobVacUsed = vacs.JobVacUsed,
                                          MaxJobVacancies = vacs.MaxJobVacancies


                                      }).ToListAsync();
            return distribution;
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
