namespace Domain.Repositories
{
    public interface ICountryIsoRepository
    {
        public string GetIsobyCountryId(int countryId);
    }
}
