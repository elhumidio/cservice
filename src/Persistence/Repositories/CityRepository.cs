using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class CityRepository : ICityRepository
    {
        private DataContext _dataContext;

        public CityRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public string GetName(int cityId)
        {
            var cityName = string.Empty;
            var city = _dataContext.Cities.FirstOrDefault(c => c.Idcity == cityId);
                if (city != null)
                cityName = city.Name;
            return cityName;
        }

        public int GetCityId(string cityName)
        {
            return _dataContext.Cities.FirstOrDefault(a => a.Name == cityName)?.Idcity ?? -1;
        }

        public async Task<int> Add(City _city)
        {

            try
            {
                var a = _dataContext.Add(_city).Entity;
                var ret = await _dataContext.SaveChangesAsync();
                return _city.Idcity;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

    }
}
