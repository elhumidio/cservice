using Domain.Entities;

namespace Domain.Repositories
{
    public interface IZipCodeRepository
    {
        public int GetZipCodeIdByCodeAndCountry(string zipcode, int country);

        public int GetZipCodeIdByCity(string cityName);

        int GetCityIdByZip(string zipcode);

        public ZipCode GetZipCodeEntity(string zipcode, int countryId);

        public int Add(ZipCode _zipcode);

        public int GetCityIdByName(string _cityName);

        public ZipCode GetZipById(int zipcodeId);
        public int GetIdCityByZipCodeAnCountry(string zipcode, int countryId);
    }
}
