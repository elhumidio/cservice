using Domain.Repositories;

namespace Persistence.Repositories
{
    public class RegEnterpriseContractRepository : IRegEnterpriseContractRepository
    {
        DataContext _dataContext;

        public RegEnterpriseContractRepository(DataContext dataContext)
        {

            _dataContext = dataContext;
        }
        public Task<int> UpdateUnits(int contractId, int jobTypeId)
        {
            var ent = _dataContext.RegEnterpriseContracts.Where(reg => reg.Idcontract == contractId && reg.IdjobVacType == jobTypeId).FirstOrDefault();
            if (ent != null)
                ent.UnitsUsed++;
            var ret = _dataContext.SaveChangesAsync();
            return Task.FromResult(ret.Result);
        }

        public Task<int> ReduceUnits(int contractId, int jobTypeId)
        {
            var ent = _dataContext.RegEnterpriseContracts.Where(reg => reg.Idcontract == contractId && reg.IdjobVacType == jobTypeId).FirstOrDefault();
            if (ent != null)
                ent.UnitsUsed--;
            var ret = _dataContext.SaveChangesAsync();
            return Task.FromResult(ret.Result);
        }

    }
}
