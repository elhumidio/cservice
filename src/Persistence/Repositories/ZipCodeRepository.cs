using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class ZipCodeRepository : IZipCodeRepository
    {
        private DataContext _dataContext;

        public ZipCodeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int GetZipCodeIdByCodeAndCountry(string zipcode, int countryId)
        {
            int zipCodeId = 0;
            var code = _dataContext.ZipCodes.Where(z => z.Zip == zipcode.Trim() && z.Idcountry == countryId).FirstOrDefault();
            if (code != null)
            {
                zipCodeId = code.IdzipCode;
            }

            return zipCodeId;
        }

        public ZipCode GetZipCodeEntity(string zipcode, int countryId)
        {
            if (zipcode == null)
                return null;

            var zipCode = _dataContext.ZipCodes.Where(z => z.Zip == zipcode.Trim() && z.Idcountry == countryId).FirstOrDefault();
            return zipCode;
        }

        public ZipCode GetZipById(int zipcodeId)
        {
            var zipCode = _dataContext.ZipCodes.Where(z => z.IdzipCode == zipcodeId).FirstOrDefault();
            return zipCode;
        }

        public int GetCityIdByZip(string zipcode)
        {
            int cityId = 0;
            var code = _dataContext.ZipCodes.Where(z => z.Zip == zipcode);
            if (code.Any())
                cityId = (int)code.First().Idcity;

            return cityId;
        }

        public int GetCityIdByName(string _cityName)
        {
            int cityId = 0;
            var code = _dataContext.Cities.Where(z => z.Name == _cityName);
            if (code.Any())
                cityId = (int)code.First().Idcity;
            return cityId;
        }

        public int Add(ZipCode _zipcode)
        {
            _dataContext.ZipCodes.Add(_zipcode);
            var ret = _dataContext.SaveChanges();
            return _zipcode.IdzipCode;
        }

        public ZipCode GetZipCodeByCityName(string cityName)
        {
            return _dataContext.ZipCodes.Where(z => z.City.Contains(cityName)).FirstOrDefault();
        }

        public int GetZipCodeIdByCity(string cityName)
        {
            var zipCodeId = 0;
            var code = _dataContext.ZipCodes.Where(z => z.City.Contains(cityName)).FirstOrDefault();
            if (code != null)
                zipCodeId = code.IdzipCode;
            return zipCodeId;
        }

        public int GetIdCityByZipCodeAnCountry(string zipcode, int countryId) {
            int cityId = -1;
            var code = _dataContext.ZipCodes.Where(z => z.Zip == zipcode && z.Idcountry == countryId).FirstOrDefault();
            if (code != null)
                cityId = (int)code.Idcity;
            return cityId;
        }

        public async Task<ZipCode> GetZipCodeByZipAndCountry(string zip, int country)
        {
            var zipcode = _dataContext.ZipCodes.FirstOrDefault(z => z.Zip == zip && z.Idcountry == country);
            return zipcode;
        }

    }
}
