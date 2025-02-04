using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class ProductLineRepository : IProductLineRepository
    {
        private DataContext _dataContext;

        public ProductLineRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int GetProductLineDuration(int idProduct)
        {
            var productLine = _dataContext.ProductLines.Where(pl => pl.Idproduct == idProduct && pl.IdjobVacType != null).FirstOrDefault();
            return productLine == null ? 0 : productLine.Duration;
        }

        public IEnumerable<ProductLine> GetProductLinesByProductId(int idProduct)
        {
            var productLines = _dataContext.ProductLines.Where(pl => pl.Idproduct == idProduct);
            return productLines;

        }
    }
}
