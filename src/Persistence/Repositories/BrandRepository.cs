using Domain.Repositories;

namespace Persistence.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        DataContext _dataContext;

        public BrandRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<int> GetBrands(int companyId)
        {
            var brands = new List<int>();
            var activebrands = _dataContext.Brands.Where(b => b.Identerprise == companyId && (bool)b.Active).Select(br => br.Idbrand).ToList();
            if (activebrands != null && activebrands.Any())
                brands = activebrands;
            return brands;

        }

        public bool IsRightBrand(int _brandId, int _enterpriseId)
        {
            var brand = _dataContext.Brands.Where(b => b.Idbrand == _brandId && b.Identerprise == _enterpriseId);
            return brand.Any();
        }
    }
}
