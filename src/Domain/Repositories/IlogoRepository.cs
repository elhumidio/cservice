using Domain.Entities;

namespace Domain.Repositories
{
    public interface IlogoRepository
    {
        public Logo GetLogoByBrand(int brandId);
    }
}
