namespace Application.ContractProducts.DTO
{
    public class CreditsAvailableByTypeCompanyAndUsers
    {
        public List<CreditsAvailableByTypeCompanyAndEnterpriseUser> CreditsByUsers { get; set; } = new List<CreditsAvailableByTypeCompanyAndEnterpriseUser>();
    }
}
