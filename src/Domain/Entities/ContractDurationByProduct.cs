namespace Domain.Entities
{
    public partial class ContractDurationByProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CountryId { get; set; }
        public int From { get; set; }
        public int? To { get; set; }
        public int? Duration { get; set; }
    }
}
