using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly DataContext _dataContext;

        public CurrencyRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public string GetCurrencyById(int currencyId, int siteId, int languageId)
        {
            var currency = _dataContext.Currencies.FirstOrDefault(c => c.IDCurrency == currencyId && c.IDSite == siteId && c.IDSLanguage == languageId);
            if (currency == null)
            {
                return string.Empty;
            }
            else
            {
                return currency.BaseName;
            }
        }
    }
}
