using Application.DTO.GeoNames;

namespace Application.Interfaces
{
    public interface IGeoNamesConector
    {
        public GeoNamesDTO GetPostalCodesCollection(string postalCode, string country);
    }
}
