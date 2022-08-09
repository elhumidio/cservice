using Domain.Repositories;
using Persistence.Enums;

namespace Persistence.Repositories
{
    public class EnterpriseRepository : IEnterpriseRepository
    {
        DataContext _dataContext;

        public EnterpriseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }



        public bool IsRightCompany(int enterpriseId)
        {
            var enterprises = _dataContext.Enterprises.Where(e => e.Identerprise == enterpriseId
            && e.ChkActive != null
            && e.ChkActive == true
            && e.Idstatus == (int)EnterpriseStatus.Active);
            return enterprises != null && enterprises.Any();
        }

        public bool UpdateATS(int enterpriseId)
        {
            var company = _dataContext.Enterprises.Where(e => e.Identerprise == enterpriseId).FirstOrDefault();
            if (company != null)
                company.Ats = true;
            return company != null;

        }
    }
}

