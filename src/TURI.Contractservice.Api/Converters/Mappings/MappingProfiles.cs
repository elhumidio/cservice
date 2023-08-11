using AutoMapper;
using Domain.Entities;
using TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder;


namespace TURI.Contractservice.Converters.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ContractProduct, ContractProductResponse>();
            CreateMap<Contract, ContractResponse>();
            CreateMap<ProductLine, ProductLineResponse>();
            CreateMap<RegEnterpriseConsum, RegEnterpriseConsumResponse>();
            CreateMap<RegEnterpriseContract, RegEnterpriseContractResponse>();
        }
    }
}
