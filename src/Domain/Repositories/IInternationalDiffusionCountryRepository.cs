using Domain.Entities;
using Domain.Enums;

namespace Domain.Repositories
{
    public interface IInternationalDiffusionCountryRepository
    {
        public Task<bool> Add(List<InternationalDiffusionCountry> list);
        public Task<bool> RemoveByOffer(int offerId);
        public Task<List<InternationalDiffusionCountry>> GetInternationalDiffusionCountriesByOffer(int offerId);
    }
}
