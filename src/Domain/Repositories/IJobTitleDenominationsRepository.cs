
using Domain.DTO;
using Domain.EnterpriseDtos;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IJobTitleDenominationsRepository
    {
        public JobTitleDenomination GetDefaultDenomination(int idJobTitle, int idSite);

        public List<JobTitleDenomination> GetAllForArea(int idArea, int idSite);
        public int GetAreaByJobTitle(int titleId);
        public IQueryable<JobTitle> GetAll();
        public Task<List<JobTitleDenominationsDto>> GetAllDenominations();
        public Task<List<JobTitleDenominationsDto>> GetAllDenominationsByLanguage(int languageId);
        public Task<List<JobTitleDenominationsDto>> GetAllDenominationsActiveOffersByLanguage(int languageId);

    }
}
