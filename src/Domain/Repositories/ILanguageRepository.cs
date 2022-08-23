using Domain.Entities;

namespace Domain.Repositories
{
    public interface ILanguageRepository
    {

        public IQueryable<TsturijobsLang> GetLanguages(int siteId);
    }
}
