namespace Application.EnterpriseContract.DTO
{
    public class CompanyinfoDto
    {
        public int CompanyId { get; set; }
        public List<int>? Brands { get; set; }
        public int IDSUser { get; set; }
        public int IDEnterpriseUser { get; set; }
        public string? UserEmail { get; set; }
        public int SiteId { get; set; }
        public string? AccountStatus { get; set; }
    }
}
