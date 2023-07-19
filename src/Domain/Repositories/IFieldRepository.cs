using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IFieldRepository
    {
        public string GetFieldById(int fieldId, int siteId, int languageId);
    }
}
