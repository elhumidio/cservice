using API.DataContext;
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

            //crear dto enterprise

        }

    }
}
