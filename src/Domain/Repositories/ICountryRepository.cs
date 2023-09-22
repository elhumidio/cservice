using Domain.Entities;

namespace Domain.Repositories
{
    public interface ICountryRepository
    {
        public bool IsRightCountry(int countryId);

        public IQueryable<Country> GetCountries(int siteId, int languageId);
        public Country GetCountryById(int countryId);
        public string GetCountryNameById(int countryId);
    }
}
