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
            .Where(o => (bool)o.pl.ChkActive
            && o.ppl.Identerprise == companyId
            && o.pl.IdstypeUser == (int)UserTypes.AdministradorEmpresa)
            .OrderByDescending(a => a.ppl.IdenterpriseUser)
            .Select(o =>
                new UserInfoDto()
                {
                    Email = o.pl.Email,
                    IDSUser = o.pl.Idsuser
                }).FirstOrDefault();

            return res;
        }

        public List<EnterpriseAdmin> GetCompanyAdmins(int companyId, bool allowManagers = false)
        {
            return _dataContext.EnterpriseUsers
                .Join(_dataContext.Users, eu => eu.Idsuser, u => u.Idsuser, (eu, u) => new { eu, u })
                .Where(entity => (bool)entity.u.ChkActive!
                    && entity.eu.Identerprise == companyId
                    && (entity.u.IdstypeUser == (int)UserTypes.AdministradorEmpresa
                        || (allowManagers && entity.u.IdstypeUser == (int)UserTypes.GestorEmpresa)))
                .Select(entity => new EnterpriseAdmin
                {
                    EnterpriseID = companyId,
                    UserId = entity.eu.IdenterpriseUser,
                    IsAdmin = entity.u.IdstypeUser == (int)UserTypes.AdministradorEmpresa,
                    IsManager = entity.u.IdstypeUser == (int)UserTypes.GestorEmpresa                    
                })
                .ToList();
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
