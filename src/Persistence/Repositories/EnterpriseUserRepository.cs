using Domain.DTO;
using Domain.Enums;
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

        public UserInfoDto GetIDSUserByCompanyId(int companyId)
        {
            var res = _dataContext.EnterpriseUsers
            .Join(_dataContext.Users, ppl => ppl.Idsuser, pl => pl.Idsuser, (ppl, pl) => new { ppl, pl })
            .Where(o => (bool)o.pl.ChkActive && o.ppl.Identerprise == companyId && o.pl.IdstypeUser == (int)UserTypes.AdministradorEmpresa)
            .OrderByDescending(a => a.ppl.IdenterpriseUser)
            .Select(o =>
                new UserInfoDto()
                {
                    Email = o.pl.Email,
                    IDSUser = o.pl.Idsuser
                }).FirstOrDefault();

            return res;
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
