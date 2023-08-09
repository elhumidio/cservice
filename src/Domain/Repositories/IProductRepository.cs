using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        public int GetProductDuration(int idProduct);
        public Product Get(int idProduct);
    }
}
