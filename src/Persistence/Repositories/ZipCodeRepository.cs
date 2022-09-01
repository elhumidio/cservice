using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class ZipCodeRepository : IZipCodeRepository
    {
        DataContext _dataContext;

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

            var zipCode = _dataContext.ZipCodes.Where(z => z.Zip == zipcode.Trim() && z.Idcountry == countryId).FirstOrDefault();
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

        public int GetZipCodeIdByCity(string cityName)
        {
            var zipCodeId = 0;
            var code = _dataContext.ZipCodes.Where(z => z.City.Contains(cityName)).FirstOrDefault();
            if (code != null)
                zipCodeId = code.IdzipCode;
            return zipCodeId;
        }
    }
}
