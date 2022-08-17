namespace Domain.Repositories
{
    public interface ICountryStateEQRepository
    {
        public Task<int> GetCountryStateEQ(string countryId);
    }
}
