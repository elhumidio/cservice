using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class RegEnterpriseContractRepository : IRegEnterpriseContractRepository
    {
        private DataContext _dataContext;

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

        public Task<int> IncrementAvailableUnits(int contractId, int jobTypeId)
        {
            var ent = _dataContext.RegEnterpriseContracts.Where(reg => reg.Idcontract == contractId && reg.IdjobVacType == jobTypeId).FirstOrDefault();
            if (ent != null)
                ent.UnitsUsed--;
            var ret = _dataContext.SaveChangesAsync();
            return Task.FromResult(ret.Result);
        }

        public Task<int> GetUnitsByType(int contractId, VacancyType type)
        {
            var units = 0;
            var unitsReg = _dataContext.RegEnterpriseContracts.Where(reg => reg.Idcontract == contractId && reg.IdjobVacType == (int)type);
            if (unitsReg.Any())
                units = unitsReg.First().Units;
            return Task.FromResult(units);
        }

        public int GetUnitsByCreditType(int contractId, VacancyTypesCredits type)
        {
            var units = 0;
            var unitsReg = _dataContext.RegEnterpriseContracts.Where(reg => reg.Idcontract == contractId && reg.IdjobVacType == (int)type);
            if (unitsReg.Any())
                units = unitsReg.First().Units;
            return units;
        }

        public List<RegEnterpriseContract> GetRegByContract(int contractId)
        {
            var cp = _dataContext.RegEnterpriseContracts.Where(reg => reg.Idcontract == contractId).ToList();
            return cp;
        }

        public async Task<int> Add(RegEnterpriseContract regContract)
        {
            var ret = await _dataContext.Set<RegEnterpriseContract>().AddAsync(regContract);
            return regContract.Idcontract;
        }
    }
}
