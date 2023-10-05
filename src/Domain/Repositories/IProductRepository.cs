using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        public int GetProductDuration(int idProduct);
        public IQueryable<Product> Get(int idProduct);
        public Task<ProductCountryPrice> GetPriceByProductIdAndCountryId(int idProduct, int idCountry);
    }
}
