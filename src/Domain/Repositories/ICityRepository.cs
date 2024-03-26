using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICityRepository
    {
        public string GetName(int cityId);
        public Task<int> Add(City _zip);
        public int GetCityId(string cityName);
    }
}
