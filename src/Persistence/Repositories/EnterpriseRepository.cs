using Domain.Enums;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class EnterpriseRepository : IEnterpriseRepository
    {
        private DataContext _dataContext;

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
            _dataContext.SaveChanges();
            return company != null;
        }

        public int GetSite(int companyId)
        {
            int site = 0;
            var company = _dataContext.Enterprises.Where(e => e.Identerprise == companyId && e.ChkActive == true).FirstOrDefault();
            if (company != null)
            {
                site = (int)company.SiteId;
            }
            return site;
        }
    }
}
