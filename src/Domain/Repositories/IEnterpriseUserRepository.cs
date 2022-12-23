using Domain.DTO;

namespace Domain.Repositories
{
    public interface IEnterpriseUserRepository
    {
        public int GetCompanyIdByUserId(int userid);

        public int GetCompanyUserIdByUserId(int userid);
        public UserInfoDto GetIDSUserByCompanyId(int companyId);
    }
}
