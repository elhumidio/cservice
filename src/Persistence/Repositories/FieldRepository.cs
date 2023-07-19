using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class FieldRepository : IFieldRepository
    {
        private readonly DataContext _dataContext;

        public FieldRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public string GetFieldById(int fieldId, int siteId, int languageId)
        {
            var field = _dataContext.Fields.FirstOrDefault(c => c.IDField == fieldId && c.IDSite == siteId && c.IDSLanguage == languageId);
            if (field == null)
            {
                return string.Empty;
            }
            else
            {
                return field.BaseName;
            }
        }
    }
}
