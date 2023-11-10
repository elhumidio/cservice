using Domain.DTO.Products;
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



        public async Task<List<ProductsPricesByQuantityAndCountryDto>> GetPricesByQuantityAndCountry(List<ProductUnits> products, int idCountry = 40)
        {
            var prices = new List<ProductsPricesByQuantityAndCountryDto>();
            foreach (var pu in products)
            {
                var priceBeforeDiscount = await _dataContext.Discounts.Where(p => p.ProductId == pu.Idproduct
                && p.CountryId == idCountry).FirstOrDefaultAsync();
                var price = await _dataContext.Discounts.Where(p => p.ProductId == pu.Idproduct
                && pu.Units >= p.From
                && pu.Units <= p.To
                && p.CountryId == idCountry).FirstOrDefaultAsync();
                var prod = new ProductsPricesByQuantityAndCountryDto
                {
                    TotalPriceAfterDiscount = pu.Units * price.UnitPrice,
                    TotalPriceBeforeDiscount = pu.Units * priceBeforeDiscount.UnitPrice,
                    UnitPriceBeforeDiscount = priceBeforeDiscount.UnitPrice,
                    UnitPriceAfterDiscount = price.UnitPrice,
                    ProductId = pu.Idproduct,
                    CountryId = idCountry,
                    DiscountPercentage = (int)price.DiscountPercent,
                    Units = pu.Units,
                    From = price.From,
                    To = price.To
                };
                prices.Add(prod);
            }
            return prices;
        }

        public async Task<List<ProductsPricesByQuantityAndCountryDto>> GetAllPricesByQuantityOrProduct(int idCountry = 40, int productId = 0)
        {
            var query = _dataContext.Discounts
                .Where(a => (productId == 0 || a.ProductId == productId) && (idCountry == 0 || a.CountryId == idCountry))
                .Select(a => new ProductsPricesByQuantityAndCountryDto
                {
                    CountryId = idCountry,
                    DiscountPercentage = (int)a.DiscountPercent,
                    ProductId = a.ProductId,
                    TotalPriceAfterDiscount = a.UnitPrice,
                    TotalPriceBeforeDiscount = a.UnitPrice,
                    UnitPriceAfterDiscount = a.UnitPrice,
                    UnitPriceBeforeDiscount = a.UnitPrice,
                    From = a.From,
                    To = a.To,
                    Units = 1
                });

            var prices = await query.ToListAsync();

            return prices;
        }

    }
}
