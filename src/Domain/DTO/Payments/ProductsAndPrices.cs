namespace Domain.DTO.Payments
{
    public class ProductsAndPrices
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class ListProductsAndPrices
    {
        public List<ProductsAndPrices> ProductsAndPrices { get; set; }
    }
}
