namespace Domain.Repositories
{
    public interface IUserRepository
    {
        public int GetUserIdByEmail(string email);
        bool IsAdmin(int userId);
    }
}
