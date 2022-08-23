using API.DataContext;
using Application.ContractProducts.DTO;
using Application.Contracts.DTO;
using Application.EnterpriseContract.DTO;
using Application.JobOffer.Commands;
using Application.JobOffer.DTO;
using Application.AuxiliaryData.DTO;
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
            CreateMap<JobVacancy, CreateOfferCommand>();
            //  CreateMap<CreateOfferCommand, JobVacancy>();
            CreateMap<CreateOfferCommand, JobVacancy>().MapOnlyIfChanged().ForMember(a => a.IdjobVacancy, opt => opt.Ignore());
            CreateMap<JobVacancy, CreateOfferCommand>().MapOnlyIfChanged().ForMember(a => a.IdjobVacancy, opt => opt.Ignore());
            CreateMap<RegJobVacMatching, RegJobVacMatchingDto>();
            CreateMap<Area, AreaDto>();
            CreateMap<Degree, DegreeDto>();
            CreateMap<Brand, BrandDto>();
            CreateMap<JobCategory, JobCategoryDto>();
            CreateMap<JobContractType, JobContractTypeDto>();
            CreateMap<JobExpYear, JobExpYearDto>();
            CreateMap<Salary, SalaryDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<Region, RegionDto>();
            CreateMap<JobVacType, JobVacTypeDto>();
            CreateMap<ResidenceType, ResidenceTypeDto>();
            CreateMap<SalaryType, SalaryTypeDto>();

            //crear dto enterprise

        }

    }
}
