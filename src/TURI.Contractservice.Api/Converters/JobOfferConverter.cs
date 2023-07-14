using Domain.Classes;
using Domain.DTO;
using TURI.ContractService.Contract.Models;

namespace API.Converters
{
    public static class JobOfferConverter
    {
        public static JobOfferResponse ToModel(this JobDataDefinition item)
        {
            return new JobOfferResponse
            {
                Title = item.Title,
                CompanyName = item.CompanyName,
                IDCountry = item.IDCountry,
                IDRegion = item.IDRegion,
                IDArea = item.IDArea,
                IDJobVacancy = item.IDJobVacancy,
                IDEnterprise = item.IDEnterprise,
                PublicationDate = item.PublicationDate,
                IDCity = item.IDCity,
                Description = item.Description,
                IDSite = item.IDSite,
                City = item.City
            };
        }

        public static OfferInfoMinForViewResponse ToModel(this OfferInfoMin item)
        {
            return new OfferInfoMinForViewResponse
            {
                Title = item.Title,
                CompanyName = item.CompanyName,
                JobId = item.JobId,
                CvId = item.CvId,
                RegistrationId = item.RegistrationId,
                CityId = item.CityId,
                CityName = item.CityName,
                CountryId = item.CountryId,
                CountryName = item.CountryName,
                Description = item.Description,
                LogoUrl = item.LogoUrl,
                NumApplies = item.NumApplies,
                RegionId = item.RegionId,
                RegionName = item.RegionName,
            };
        }
    }
}
