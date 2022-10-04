using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class LogoRepository : IlogoRepository
    {
        private DataContext _dataContext;

        public LogoRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Logo GetLogoByBrand(int brandId)
        {
            var logo = _dataContext.Logos.Where(l => l.Idbrand == brandId).FirstOrDefault();
            return logo;

        }
    }
}
