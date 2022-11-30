using Domain.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ITitleRepository
    {
        public Task<List<TitleLang>> GetByLanguage(int langId);
    }
}
