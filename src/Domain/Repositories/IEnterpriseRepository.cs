namespace Domain.Repositories
{
    public interface IEnterpriseRepository
    {
        public bool IsRightCompany(int enterpriseId);
        public bool UpdateATS(int enterpriseId);
    }
}
