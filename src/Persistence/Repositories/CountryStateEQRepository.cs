using Domain.Enums;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class CountryStateEQRepository : ICountryStateEQRepository
    {
        private readonly DataContext _dataContext;

        public CountryStateEQRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<int> GetCountryStateEQ(string countryId)
        {
            var eq = _dataContext.EquestCountryStates.Where(c => c.IdcountryState == countryId).FirstOrDefault();
            if (eq != null)
                return Task.FromResult((int)eq.EquivalentId);
            else return Task.FromResult((int)Regions.AllCountry);
        }
    }
}
