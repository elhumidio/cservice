using Domain.DTO.Products;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        public int GetProductDuration(int idProduct);
        public IQueryable<Product> Get(int idProduct);
        public Task<ProductCountryPrice> GetPriceByProductIdAndCountryId(int idProduct, int idCountry);
        public Task<List<ProductsPricesByQuantityAndCountryDto>> GetPricesByQuantityAndCountry(List<ProductUnits> idProducts, int idCountry = 40);
        public Task<List<ProductsPricesByQuantityAndCountryDto>> GetAllPricesByQuantityOrProduct(int idCountry = 40,int productId = 0);


    }
}
