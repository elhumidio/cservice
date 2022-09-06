using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private DataContext _dataContext;

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

        public IQueryable<Brand> GetListBrands(int companyId)
        {
            var brands = _dataContext.Brands
                .Where(a => a.Identerprise == companyId && a.Active == true);

            if (brands != null)
            {
                return brands;
            }
            else
            {
                return null;
            }
        }

        public bool IsRightBrand(int _brandId, int _enterpriseId)
        {
            var brand = _dataContext.Brands.Where(b => b.Idbrand == _brandId
            && b.Identerprise == _enterpriseId
            && b.Idbrand > 0
            && b.Active == true);
            return brand.Any();
        }
    }
}
