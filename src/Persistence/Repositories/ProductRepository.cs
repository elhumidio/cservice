using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private DataContext _dataContext;

        public ProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Product Get(int idProduct)
        {
            var product = _dataContext.Products.FirstOrDefault(p => p.Idproduct == idProduct);
            return product;
        }

        public int GetProductDuration(int idProduct)
        {
            var product = _dataContext.Products.Where(p => p.Idproduct == idProduct).FirstOrDefault();
            return product == null ? 0 : product.Duration;
        }
    }
}
