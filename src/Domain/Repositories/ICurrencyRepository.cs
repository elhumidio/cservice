namespace Domain.Repositories
{
    public interface ICurrencyRepository
    {
        public string GetCurrencyById(int currencyId, int siteId, int languageId);
    }
}
