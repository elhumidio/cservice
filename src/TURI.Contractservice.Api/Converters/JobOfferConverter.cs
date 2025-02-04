using Application.JobOffer.DTO;
using Application.JobOffer.Queries;
using Domain.Classes;
using Domain.DTO;
using Domain.DTO.ManageJobs;
using Domain.Entities;
using TURI.ContractService.Contract.Models;
using TURI.ContractService.Contracts.Contract.Models.ManageJobs;
using TURI.ContractService.Contracts.Contract.Models.Response;
using JobOfferResponse = TURI.ContractService.Contract.Models.JobOfferResponse;

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
                UpdatingDate = item.UpdatingDate,
                IDCity = item.IDCity,
                Description = item.Description,
                IDSite = item.IDSite,
                City = item.City,
                ChkBlindVac = item.ChkBlindVac,
                TitleId = item.TitleId,
            };
        }

        public static GetOffersForDashBoardRequest ToDomain(this GetOffersForDashBoardQuery item)
        {
            return new GetOffersForDashBoardRequest
            {
                Actives = item.Actives,
                All = item.All,
                CompanyId = item.CompanyId,
                Filed = item.Filed,
                LangId = item.LangId,
                Page = item.Page,
                PageSize = item.PageSize,
                Site = item.Site,
                BrandId = item.BrandId,
                Location = item.Location,
                Title = item.Title
            };
        }

        public static ManageJobsResponse ToResponse(this ManageJobsDto item)
        {
            return new ManageJobsResponse
            {
                Offers = item.Offers.Select(a => a.ToResponse()).ToArray(),
            };
        }

        public static OfferModelResponse ToResponse(this OfferModel item)
        {
            return new OfferModelResponse
            {
                AreaName = item.AreaName,
                Caducity = item.Caducity,
                CaducityShow = item.CaducityShow,
                CCAA = item.CCAA,
                chkAllCountry = item.chkAllCountry,
                ChkBlindVac = item.ChkBlindVac,
                ChkColor = item.ChkColor,
                ChkDeleted = item.ChkDeleted,
                ChkEnterpriseVisible = item.ChkEnterpriseVisible,
                ChkFilled = item.ChkFilled,
                ChkPack = item.ChkPack,
                ChkUpdateDate = item.ChkUpdateDate,
                City = item.City,
                CityUrl = item.CityUrl,
                ContractFinishDate = item.ContractFinishDate,
                ContractStartDate = item.ContractStartDate,
                DegreeName = item.DegreeName,
                EnterpriseName = item.EnterpriseName,
                ExpYears = item.ExpYears,
                ExtensionDays = item.ExtensionDays,
                FieldName = item.FieldName,
                FinishDate = item.FinishDate,
                FormData = item.FormData,
                Idbrand = item.Idbrand,
                Idcity = item.Idcity,
                Idcontract = item.Idcontract,
                Idcountry = item.Idcountry,
                Identerprise = item.Identerprise,
                IdenterpriseUserG = item.IdenterpriseUserG,
                IdjobRegType = item.IdjobRegType,
                IdjobVacancy = item.IdjobVacancy,
                IdjobVacType = item.IdjobVacType,
                IDProduct = item.IDProduct,
                Idregion = item.Idregion,
                Idsite = item.Idsite,
                Idstatus = item.Idstatus,
                isCancel = item.isCancel,
                IsOldOffer = item.IsOldOffer,
                isPending = item.isPending,
                IsWelcome = item.IsWelcome,
                JobRegType = item.JobRegType,
                JobVacType = item.JobVacType,
                Name = item.Name,
                NDescartados = item.NDescartados,
                NEvaluating = item.NEvaluating,
                NFinalistas = item.NEvaluating,
                NNuevos = item.NNuevos,
                NPendientes = item.NPendientes,
                OfferUrl = item.OfferUrl,
                PublicationDate = item.PublicationDate,
                RegionName = item.RegionName,
                RegNumber = item.RegNumber,
                RegPercent = item.RegPercent,
                SalaryMin1 = item.SalaryMin1,
                SalaryType = item.SalaryType,
                SubdomainName = item.SubdomainName,
                Title = item.Title,
                UpdatingDate = item.UpdatingDate,
                ZipCode = item.ZipCode,
                ZipCodeCity = item.ZipCodeCity,
                CanSeeFilters = item.CanSeeFilters
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

        public static CompanyOffersPerDayResponse ToResponse(this CompanyOffersPerDayDto item)
        {
            return new CompanyOffersPerDayResponse
            {
                Date = item.Date,
                EnterpriseId = item.EnterpriseId,
                OffersPublished = item.OffersPublished
            };
        }

        public static OfferInfoBuildUrlResponse ToResponse(this JobVacancy item)
        {
            return new OfferInfoBuildUrlResponse
            {
                OfferId = item.IdjobVacancy,
                Title = item.Title,
                RegionId = item.Idregion,
                CityId = item.Idcity ?? 0,
                SiteId = item.Idsite
            };
        }
    }
}
