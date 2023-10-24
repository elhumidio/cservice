namespace Domain.Entities
{
    public partial class ProductCountryPrice
    {
        public int Idproduct { get; set; }
        public int Idcountry { get; set; }
        public int? Price { get; set; }
        public int? Discount { get; set; }
        public int Id { get; set; }
    }
}
