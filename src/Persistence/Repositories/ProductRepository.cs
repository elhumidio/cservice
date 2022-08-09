using Domain.Repositories;

namespace Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        DataContext _dataContext;

        public ProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public int GetProductDuration(int idProduct)
        {
            var product = _dataContext.Products.Where(p => p.Idproduct == idProduct).FirstOrDefault();
            return product == null ? 0 : product.Duration;
        }
    }
}
