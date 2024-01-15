using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class InternationalDiffusionCountryRepository : IInternationalDiffusionCountryRepository
    {
        private DataContext _dataContext;

        public InternationalDiffusionCountryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> Add(List<InternationalDiffusionCountry> list)
        {

            await _dataContext.AddRangeAsync(list);
            var ret = _dataContext.SaveChanges();
            return ret > 0;
        }

        public async Task<bool> RemoveByOffer(int offerId)
        {
            var countriesToRemove = _dataContext.InternationalDiffusionCountries
                                         .Where(r => r.OfferId == offerId);

            _dataContext.InternationalDiffusionCountries.RemoveRange(countriesToRemove);

            int affectedRows = await _dataContext.SaveChangesAsync();

            return affectedRows > 0;
        }

        public async Task<List<InternationalDiffusionCountry>> GetInternationalDiffusionCountriesByOffer(int offerId)
        {
            return await _dataContext.InternationalDiffusionCountries.Where(r => r.OfferId == offerId).ToListAsync<InternationalDiffusionCountry>();
        }
    }
}
