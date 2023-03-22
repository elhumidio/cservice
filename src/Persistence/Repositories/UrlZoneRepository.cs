using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UrlZoneRepository : IZoneUrl
    {
        private readonly DataContext _dataContext;

        public UrlZoneRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public string GetCityUrlByCityId(int cityId)
        {   
                var url = _dataContext.ZoneUrls.Where(a => a.Idcity == cityId).FirstOrDefault().Url;
                return url;            
        }
    }
}
