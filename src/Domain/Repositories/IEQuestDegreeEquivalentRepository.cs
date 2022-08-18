namespace Domain.Repositories
{
    public interface IEQuestDegreeEquivalentRepository
    {
        public Task<int> GeteQuestDegree(int degreeId, int siteId);
    }
}
