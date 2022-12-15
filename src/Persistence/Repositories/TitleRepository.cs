using Domain.DTO;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class TitleRepository : ITitleRepository
    {
        private readonly DataContext _dataContext;
        public TitleRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<TitleLang>> GetByLanguage(int langId)
        {
            var titles = await _dataContext.TitleLangs.Include("Title").Where(a => a.LanguageId == langId).ToListAsync();
            return titles;

             
        }
    }
}
