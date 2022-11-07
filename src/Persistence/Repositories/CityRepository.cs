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
    }
}
