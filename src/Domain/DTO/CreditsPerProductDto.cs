
namespace Application.ContractProducts.DTO
{
    public class CreditsPerProductDto
    {
        public int ContractId { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractFinishDate { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int TotalCredits { get; set; }
        public int ConsumedCredits { get; set; }
        public int RemainingCredits { get; set; }
    }
}
