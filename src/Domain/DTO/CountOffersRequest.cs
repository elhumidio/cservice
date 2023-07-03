namespace Domain.DTO
{
    public class CountOffersRequest
    {
        public int? LastNumberOfDays { get; set; }
        public int[] JobOfferIds { get; set; } = Array.Empty<int>();
    }
}
