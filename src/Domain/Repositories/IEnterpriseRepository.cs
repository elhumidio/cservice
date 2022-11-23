namespace Domain.Repositories
{
    public interface IEnterpriseRepository
    {
        public bool IsRightCompany(int enterpriseId);

        public bool UpdateATS(int enterpriseId);

        public int GetSite(int companyId);

        public string GetCompanyName(int companyId);

        public API.DataContext.Enterprise Get(int companyId);
    }
}
