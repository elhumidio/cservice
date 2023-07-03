namespace Domain.Repositories
{
    public interface IEnterpriseRepository
    {
        public bool IsRightCompany(int enterpriseId);

        public bool UpdateATS(int enterpriseId);

        public int GetSite(int companyId);

        public string GetCompanyName(int companyId);

        public string GetCompanyNameCheckingBlind(int companyId, bool isBlind);

        public API.DataContext.Enterprise Get(int companyId);
        public Task<int> GetCountCompaniesActive();
        public int GetCompanyRegion(int companyId);
        public string GetCompanyNameByBrandId(int brandId);
    }
}
