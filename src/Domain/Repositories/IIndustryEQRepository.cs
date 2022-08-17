namespace Domain.Repositories
{
    public interface IindustryEQRepository
    {
        Task<int> GetEQuestIndustryCode(int industryCode);
    }
}
