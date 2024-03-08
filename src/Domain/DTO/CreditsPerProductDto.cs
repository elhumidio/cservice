
namespace Application.ContractProducts.DTO
{
    public class CreditsPerProductDto
    {
        public int ContractId { get; set; }
        public int ProductId { get; set; }
        public int TotalCredits { get; set; }
        public int ConsumedCredits { get; set; }
        public int RemainingCredits { get; set; }
    }
}
