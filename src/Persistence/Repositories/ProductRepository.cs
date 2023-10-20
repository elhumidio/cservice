using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private DataContext _dataContext;

        public ProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<Product> Get(int idProduct)
        {
            var product = _dataContext.Products.Where(p => p.Idproduct == idProduct);
            return product;
        }

        public int GetProductDuration(int idProduct)
        {
            var product = _dataContext.Products.Where(p => p.Idproduct == idProduct).FirstOrDefault();
            return product == null ? 0 : product.Duration;
        }

            
        public async Task<ProductCountryPrice> GetPriceByProductIdAndCountryId(int idProduct, int idCountry)
        {            
            var price = await _dataContext.ProductCountryPrices.Where(p => p.Idproduct == idProduct && p.Idcountry == idCountry).FirstOrDefaultAsync();
            if(price == null)
            {
                price = await _dataContext.ProductCountryPrices.Where(p => p.Idproduct == idProduct && p.Idcountry == (int)CountriesTurijobsDefined.Spain).FirstOrDefaultAsync();
            }
            return price;
        }

    }
}
