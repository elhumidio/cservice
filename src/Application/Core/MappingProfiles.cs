using Application.AuxiliaryData.DTO;
using Application.ContractProducts.DTO;
using Application.Contracts.DTO;
using Application.EnterpriseContract.DTO;
using Application.JobOffer.Commands;
using Application.JobOffer.DTO;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<JobVacancy, JobOfferDto>();
            CreateMap<EnterpriseUserJobVac, UnitsAssignmentDto>();
            CreateMap<Contract, ContractDto>();
            CreateMap<RegEnterpriseContract, RegEnterpriseContractDto>();
            CreateMap<ContractProduct, ContractProductDto>();
            CreateMap<CreateOfferCommand, JobVacancy>().MapOnlyIfChanged().ForMember(a => a.IdjobVacancy, opt => opt.Ignore());
            CreateMap<JobVacancy, CreateOfferCommand>().MapOnlyIfChanged().ForMember(a => a.IdjobVacancy, opt => opt.Ignore());
            CreateMap<JobVacancy, UpdateOfferCommand>().MapOnlyIfChanged().ForMember(a => a.IdjobVacancy, opt => opt.Ignore());
            CreateMap<UpdateOfferCommand, JobVacancy>().MapOnlyIfChanged().ForMember(a => a.IdjobVacancy, opt => opt.Ignore());
            CreateMap<RegJobVacMatching, RegJobVacMatchingDto>();
            CreateMap<Area, AreaDTO>();
            CreateMap<Degree, DegreeDTO>();
            CreateMap<Brand, BrandDTO>();
            CreateMap<JobCategory, JobCategoryDTO>();
            CreateMap<JobContractType, JobContractTypeDTO>();
            CreateMap<JobExpYear, JobExpYearDTO>();
            CreateMap<Salary, SalaryDTO>();
            CreateMap<Country, CountryDTO>();
            CreateMap<Region, RegionDTO>();
            CreateMap<JobVacType, JobVacTypeDTO>();
            CreateMap<ResidenceType, ResidenceTypeDTO>();
            CreateMap<SalaryType, SalaryTypeDTO>();
            CreateMap<Site, SiteDTO>();
            CreateMap<TsturijobsLang, LanguageDTO>();
            CreateMap<JobVacancy, JobOfferDto>();
            CreateMap<JobOfferDto, JobVacancy>();
            CreateMap<JobVacancy, OfferDto>();
            CreateMap<OfferDto, JobOfferDto>();
            CreateMap<RegJobVacMatching, IntegrationData>()
                .ForMember(a => a.ApplicationReference, opt => opt.MapFrom(src => src.ExternalId))
                .ForMember(a => a.IDIntegration, opt => opt.MapFrom(src => src.Idintegration))
                .ForMember(a => a.ApplicationUrl, opt => opt.MapFrom(src => src.Redirection))
                .ForMember(a => a.ApplicationEmail, opt => opt.MapFrom(src => src.AppEmail));
            CreateMap<IntegrationData, RegJobVacMatching>()
                .ForMember(a => a.ExternalId, opt => opt.MapFrom(src => src.ApplicationReference))
                .ForMember(a => a.Idintegration, opt => opt.MapFrom(src => src.IDIntegration))
                .ForMember(a => a.Redirection, opt => opt.MapFrom(src => src.ApplicationUrl))
                .ForMember(a => a.AppEmail, opt => opt.MapFrom(src => src.ApplicationEmail));
        }
    }
}
