using Domain.Repositories;

namespace Persistence.Repositories
{
    public class CountryIsoRepository : ICountryIsoRepository
    {
        private readonly DataContext _dataContext;

        public CountryIsoRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public string GetIsobyCountryId(int countryId)
        {
            var countries = _dataContext.CountryIsos.Where(c => c.Idcountry == countryId);
            if (countries != null && countries.Any())
                return countries.FirstOrDefault().Iso;
            else return string.Empty;
        }
    }
}
