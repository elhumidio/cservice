using Domain.DTO.Products;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        public int GetProductDuration(int idProduct);
        public IQueryable<Product> Get(int idProduct);
        public Task<List<ProductsPricesByQuantityAndCountryDto>> GetPricesByQuantityAndCountry(List<ProductUnits> products, int idCountry = 40);
        public Task<List<ProductsPricesByQuantityAndCountryDto>> GetAllPricesByQuantityOrProduct(int idCountry = 40,int productId = 0);
        public string GetProductName(int idProduct);
        public Task<List<ProductsPricesByQuantityAndCountryDto>> GetAllStripeProductIds();
    }
}
