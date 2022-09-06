using Domain.Repositories;

namespace Persistence.Repositories
{
    public class EnterpriseUserRepository : IEnterpriseUserRepository
    {
        private DataContext _dataContext;

        public EnterpriseUserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int GetCompanyIdByUserId(int userid)
        {
            var companyId = 0;
            var company = _dataContext.EnterpriseUsers.Where(eu => eu.Idsuser == userid).FirstOrDefault();
            if (company != null)
                companyId = company.Identerprise;
            return companyId;
        }

        public int GetCompanyUserIdByUserId(int userid)
        {
            var companyUserId = 0;
            var company = _dataContext.EnterpriseUsers.Where(eu => eu.Idsuser == userid).FirstOrDefault();
            if (company != null)
                companyUserId = company.IdenterpriseUser;
            return companyUserId;
        }
    }
}
