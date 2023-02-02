using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class UnitsRepository : IUnitsRepository
    {
        private readonly DataContext _dataContext;

        public UnitsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<EnterpriseUserJobVac> GetAssignmentsByContract(int contractId)
        {
            var query = _dataContext.EnterpriseUserJobVacs.Where(a => a.Idcontract == contractId);
            return query;
        }

        IQueryable<EnterpriseUserJobVac> IUnitsRepository.GetAssignmentsByContractAndManager(int contractId, int manager)
        {
            var query = _dataContext.EnterpriseUserJobVacs.Where(a => a.Idcontract == contractId && a.IdenterpriseUser == manager);
            return query;
        }

        public  int GetAssignedUnitsMxPtByCompany(int companyId)
        {
            var res =  _dataContext.EnterpriseUserJobVacs
          .Join(_dataContext.Contracts, c => new { c.Idcontract }, eujv => new { eujv.Idcontract },
              (jv, c) => new { jv, c })
          .Where(o => o.c.Identerprise == companyId
          && (o.jv.Idproduct == 110 || o.jv.Idproduct == 115) && o.c.FinishDate >= DateTime.Today)
          .Select(o => o.jv).ToList();
            return res.Sum(a => a.JobVacUsed);
        }


        public async  Task<Dictionary<int, List<int>>> GetAssignedContractsForManagers(List<int> managers) {

            Dictionary<int, List<int>> dic = new ();


         /*   var res = _dataContext.Contracts
            .Join(_dataContext.EnterpriseUserJobVacs, p => new { p.Idcontract }, cp => new { cp.Idcontract },
                (p, cp) => new { p, cp })
            .Join(_dataContext, ppl => ppl.p.Idproduct, pl => pl.Idproduct, (ppl, pl) => new { ppl, pl })
            .Where(o => o.ppl.p.Idcontract == contractId && o.pl.IdjobVacType != null)
            .Select(o => o.ppl.cp.Idproduct)
            .First();
            return res;*/
            return dic;
        }

        public bool AssignUnitToManager(int contractId, VacancyType type, int ownerId)
        {
            var assignment = _dataContext.EnterpriseUserJobVacs.Where(a => a.IdjobVacType == (int)type
                && a.Idcontract == contractId
                && a.IdenterpriseUser == ownerId).FirstOrDefault();
            if (assignment != null)
            {
                assignment.JobVacUsed++;
                _dataContext.SaveChanges();
            }
            return true;
        }

        public bool TakeUnitFromManager(int contractId, VacancyType type, int ownerId)
        {
            var assignment = _dataContext.EnterpriseUserJobVacs.Where(a => a.IdjobVacType == (int)type
                && a.Idcontract == contractId
                && a.IdenterpriseUser == ownerId).FirstOrDefault();
            if (assignment != null)
            {
                assignment.JobVacUsed--;
                _dataContext.SaveChanges();
            }
            return true;
        }
    }
}
