namespace Domain.Entities
{
    public partial class Discount
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CountryId { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public decimal DiscountPercent { get; set; }
        public int UnitPrice { get; set; }
    }
}
