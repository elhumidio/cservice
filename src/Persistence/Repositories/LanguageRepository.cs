using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly DataContext _dataContext;

        public LanguageRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<TsturijobsLang> GetLanguages(int siteId)
        {
            var languages = _dataContext.TsturijobsLangs
                .Where(a => a.Idsite == siteId);

            if (languages != null)
            {
                return languages;
            }
            else
            {
                return null;
            }
        }
    }
}
