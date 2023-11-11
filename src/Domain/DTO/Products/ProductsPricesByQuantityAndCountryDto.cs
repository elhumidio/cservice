namespace Domain.DTO.Products
{
    public class ProductsPricesByQuantityAndCountryDto
    {
        public int id { get; set; }
        public int ProductId { get; set; }
        public int CountryId { get; set; }
        public decimal TotalPriceBeforeDiscount { get; set; }
        public decimal TotalPriceAfterDiscount { get; set; }
        public decimal UnitPriceBeforeDiscount { get; set; }
        public decimal UnitPriceAfterDiscount { get; set; }
        public int DiscountPercentage { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int Units { get; set; }
        public int UnitsNeededToGetDiscount { get; set; }
    }


}
