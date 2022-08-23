using Domain.Entities;

namespace Domain.Repositories
{
    public interface IBrandRepository
    {
        public bool IsRightBrand(int brandId, int enterpriseId);
        public List<int> GetBrands(int companyId);
        public IQueryable<Brand> GetListBrands(int companyId);
    }
}
