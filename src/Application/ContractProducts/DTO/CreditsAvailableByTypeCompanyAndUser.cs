using Domain.DTO.Products;

namespace Application.ContractProducts.DTO
{
    public class CreditsAvailableByTypeCompanyAndUser
    {
        public List<ProductUnitsForPublishOffer> ProductCredits { get; set; } = new List<ProductUnitsForPublishOffer>();
    }
}
