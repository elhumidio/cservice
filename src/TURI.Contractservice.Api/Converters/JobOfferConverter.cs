using Application.Contracts.DTO;
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
                IDBrand = item.IDBrand,
                IDEnterprise = item.IDEnterprise,
                ChkBlindVacancy = item.ChkBlindVacancy,
                PublicationDate = item.PublicationDate,
                City = item.City,
                IDCity = item.IDCity,
                ActiveDays = item.ActiveDays,
                Description = item.Description,
                Logo = item.Logo,
                IDSite = item.IDSite,
            };
        }
    }
}
