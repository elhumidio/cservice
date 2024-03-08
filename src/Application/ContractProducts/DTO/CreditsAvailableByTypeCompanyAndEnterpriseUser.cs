using Domain.DTO.Products;

namespace Application.ContractProducts.DTO
{
    public class CreditsAvailableByTypeCompanyAndEnterpriseUser
    {
        public int IDEnterpriseUser { get; set; }
        public List<ProductUnitsForPublishOffer> ProductCredits { get; set; } = new List<ProductUnitsForPublishOffer>();
    }
}
