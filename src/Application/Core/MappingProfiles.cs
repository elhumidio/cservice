using Application.ContractProducts.DTO;
using Application.Contracts.DTO;
using Application.EnterpriseContract.DTO;
using Application.JobOffer.Commands;
using Application.JobOffer.DTO;
using AutoMapper;
using Domain.Entities;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<JobVacancy, JobOfferDTO>();
            CreateMap<EnterpriseUserJobVac, UnitsAssignmentDTO>();
            CreateMap<Contract, ContractDTO>();
            CreateMap<RegEnterpriseContract, RegEnterpriseContractDTO>();
            CreateMap<ContractProduct, ContractProductDTO>();
            CreateMap<JobVacancy, CreateOfferCommand>();
            CreateMap<CreateOfferCommand, JobVacancy>();

            //crear dto enterprise

        }
    }
}
