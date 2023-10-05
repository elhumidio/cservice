namespace Domain.Entities
{
    public partial class ProductCountryPrice
    {
        public int Idproduct { get; set; }
        public int Idcountry { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public int Id { get; set; }
    }
}
