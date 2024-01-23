
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IJobTitleDenominationsRepository
    {
        public JobTitleDenomination GetDefaultDenomination(int idJobTitle, int idSite);

        public List<JobTitleDenomination> GetAllForArea(int idArea, int idSite);

    }
}
