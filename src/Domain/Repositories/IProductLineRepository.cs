using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProductLineRepository
    {
        public int GetProductLineDuration(int idProduct);

        public IEnumerable<ProductLine> GetProductLinesByProductId(int idProduct);
    }
}
