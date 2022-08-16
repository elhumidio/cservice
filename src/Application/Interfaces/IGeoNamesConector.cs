using Application.DTO.GeoNames;

namespace Application.Interfaces
{
    public interface IGeoNamesConector
    {
        public GeoNamesDto GetPostalCodesCollection(string postalCode, string country);
    }
}
