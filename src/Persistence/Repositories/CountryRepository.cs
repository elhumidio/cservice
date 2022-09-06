using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _dataContext;

        public CountryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool IsRightCountry(int _countryId)
        {
            var country = _dataContext.Countries.Where(a => a.Idcountry == _countryId && a.Idcountry != 0);
            return country.Any();
        }

        public IQueryable<Country> GetCountries(int siteId, int languageId)
        {
            var countries = _dataContext.Countries
                .Where(a => a.Idsite == siteId && a.Idslanguage == languageId)
                .Where(a => a.Idcountry > 0);

            if (countries != null)
            {
                return countries;
            }
            else
            {
                return null;
            }
        }
    }
}
