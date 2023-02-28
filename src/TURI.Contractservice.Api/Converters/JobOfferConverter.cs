using Domain.Classes;
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
            };
        }
    }
}
