namespace Domain.Repositories
{
    public interface IZipCodeRepository
    {
        public int GetZipCodeIdByCodeAndCountry(string zipcode, int country);
        public int GetZipCodeIdByCity(string cityName);
        int GetCityIdByZip(string zipcode);
    }
}
