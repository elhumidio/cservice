using Domain.DTO;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private DataContext _dataContext;

        public BrandRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<BrandDto>> GetAllBrands()
        {
            var brandsList = _dataContext.Brands
                .Join(_dataContext.Enterprises, b => new { b.Identerprise }, e => new { e.Identerprise }, (b, e) => new { b, e })
                .Join(_dataContext.Contracts, c => c.e.Identerprise, pl => pl.Identerprise, (c, pl) => new { c, pl })
                .Where(ent => (bool)ent.c.e.ChkActive && ent.c.e.Idstatus == 1 && ent.pl.ChkApproved && ent.pl.FinishDate > DateTime.Now.AddDays(-180))         
                .Select(a => new BrandDto {IDBrand = a.c.b.Idbrand, IDEnterprise = a.c.e.Identerprise,Name = a.c.b.Name} ).ToListAsync();

            var result = await brandsList;
            var distincted = result.DistinctBy(a => a.IDEnterprise).ToList();
            return distincted;
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
