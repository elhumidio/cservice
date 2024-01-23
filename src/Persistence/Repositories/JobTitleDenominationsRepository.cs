using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class JobTitleDenominationsRepository : IJobTitleDenominationsRepository
    {
        public DataContext _dataContext { get; }

        public JobTitleDenominationsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<JobTitleDenomination> GetAllForArea(int idArea, int idSite)
        {
            return _dataContext.JobTitleDenominations.Join(_dataContext.JobTitleAreas, inner => inner.FK_JobTitle, outer => outer.FK_JobTitleID, (inner, outer) => new
            {
                Denom = inner,
                IDArea = outer.FK_AreaID,
                IDSite = outer.FK_IDSite
            })
            .Where(v => v.IDArea == idArea && v.IDSite == idSite)
            .Select(d => d.Denom)
            .ToList();
        }

        public JobTitleDenomination GetDefaultDenomination(int idJobTitle, int idSite)
        {
            int languageId;
            switch (idSite)
            {
                case 6: case 11: //Spain, Mexico
                    languageId = 7; break;
                case 8: case 18: //Portugal, Brazil
                    languageId = 17; break;
                default:
                    languageId = 14; break;
            }

            return _dataContext.JobTitleDenominations.FirstOrDefault(d => d.ID == idJobTitle && d.LanguageId == languageId);

            
        }
    }
}
