using Domain.DTO.Products;
using Domain.Entities;
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

        public string GetProductName(int idProduct)
        {
            var product = _dataContext.Products.Where(p => p.Idproduct == idProduct).FirstOrDefault();
            return product == null ? string.Empty : product.BaseName;
        }

        public async Task<List<ProductsPricesByQuantityAndCountryDto>> GetPricesByQuantityAndCountry(List<ProductUnits> products, int idCountry = 40)
        {
            idCountry = 40;
            var prices = new List<ProductsPricesByQuantityAndCountryDto>();
            foreach (var pu in products)
            {
                //Largest per unit price
                var priceBeforeDiscount = await _dataContext.Discounts
                    .Where(p => p.ProductId == pu.Idproduct && p.CountryId == idCountry)
                    .OrderByDescending(p => p.UnitPrice)
                    .FirstOrDefaultAsync();

                //The correct Band for the number of units we have
                var price = await _dataContext.Discounts.Where(p => p.ProductId == pu.Idproduct
                        && pu.Units >= p.From
                        && pu.Units <= p.To
                        && p.CountryId == idCountry)
                    .FirstOrDefaultAsync();

                if(price != null)
                {
                    var firstLine = new ProductsPricesByQuantityAndCountryDto
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
                        To = price.To,
                        id = price.Id,
                        StripeProductId = price.StripeProductId ?? string.Empty,
                        SpecialPriceWelcome = price.WelcomeSpecialPrice ?? 0
                    };

                    prices.Add(firstLine);
                    var nextBandMinUnits = firstLine.To + 1;
                    Discount priceNext = null;
                    if (pu.Idproduct != 110)
                    {
                        priceNext = await _dataContext.Discounts
                      .Where(p => p.ProductId == pu.Idproduct && p.CountryId == idCountry && p.From == nextBandMinUnits)
                      .FirstOrDefaultAsync();
                    }


                    if (priceNext != null)
                    {
                        var secondLine = new ProductsPricesByQuantityAndCountryDto
                        {
                            TotalPriceAfterDiscount = pu.Units * priceNext.UnitPrice,
                            TotalPriceBeforeDiscount = pu.Units * priceBeforeDiscount.UnitPrice,
                            UnitPriceBeforeDiscount = priceBeforeDiscount.UnitPrice,
                            UnitPriceAfterDiscount = priceNext.UnitPrice,
                            ProductId = pu.Idproduct,
                            CountryId = idCountry,
                            DiscountPercentage = (int)priceNext.DiscountPercent,
                            Units = pu.Units,
                            From = priceNext.From,
                            To = priceNext.To,
                            id = priceNext.Id,
                            StripeProductId = string.Empty, //Not used
                            SpecialPriceWelcome = price.WelcomeSpecialPrice ?? 0
                        };
                        prices.Add(secondLine);
                        firstLine.UnitsNeededToGetDiscount = secondLine.From - firstLine.Units;
                    }
                }
                else
                {
                    continue; };
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
                    Units = 1,
                    StripeProductId = a.StripeProductId,
                    SpecialPriceWelcome = a.WelcomeSpecialPrice
                });

            var prices = await query.ToListAsync();

            return prices;
        }

        public async Task<List<ProductsPricesByQuantityAndCountryDto>> GetAllStripeProductIds()
        {
            return _dataContext.Discounts.Select(a => new ProductsPricesByQuantityAndCountryDto
            {
                id = a.Id,
                CountryId = a.CountryId,
                DiscountPercentage = (int)a.DiscountPercent,
                ProductId = a.ProductId,
                TotalPriceAfterDiscount = a.UnitPrice,
                TotalPriceBeforeDiscount = a.UnitPrice,
                UnitPriceAfterDiscount = a.UnitPrice,
                UnitPriceBeforeDiscount = a.UnitPrice,
                From = a.From,
                To = a.To,
                Units = 1,
                UnitsNeededToGetDiscount = 0,
                StripeProductId = a.StripeProductId,
                SpecialPriceWelcome = a.WelcomeSpecialPrice
            }).ToList();
        }
    }
}
