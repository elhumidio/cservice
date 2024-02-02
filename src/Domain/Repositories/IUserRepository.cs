using Domain.Classes;
using Domain.DTO;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        public int GetUserIdByEmail(string email);

        bool IsAdmin(int userId);

        public Task<List<UserContractExpireSoonData>> GetUsersContractExpireSoon(int days);

        public Task<List<UserContractAvailableUnitsData>> GetUsersContractAvailableUnits();

        public Task<List<UserContractBegin>> GetUsersContractBegin();

        public Task<List<UserGetResponseData>> ListUsersEmptyGetResponse();

        public bool UpdateUserGetResponse(User request);

        public int GetIdsuserByManagerId(int managerId);

        public List<UserDto> GetCompanyValidUsers(int companyId);
    }
}
