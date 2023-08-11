using Application.ContractCreation.Commands;
using Application.ContractCreation.Dto;
using Application.Contracts.DTO;
using AutoMapper;
using Domain.DTO.Requests;
using Domain.Entities;
using TURI.ContractService.Contract.Models;
using TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder;
using TURI.ContractService.Contracts.Contract.Models.Requests;

namespace API.Converters
{
    public static class ContractConverter
    {
        static ContractConverter()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ContractProduct, ContractProductResponse>();
                cfg.CreateMap<Contract, ContractResponse>();
                cfg.CreateMap<ProductLine, ProductLineResponse>();
                cfg.CreateMap<RegEnterpriseConsum, RegEnterpriseConsumResponse>();
                cfg.CreateMap<RegEnterpriseContract, RegEnterpriseContractResponse>();
                cfg.CreateMap<UpsertContractCommand, ContractCreateRequest>();
            });

            _mapper = configuration.CreateMapper();
        }

        private static IMapper _mapper;

        public static AvailableUnitsResponse ToModel(this AvailableUnitsDto item)
        {
            return new AvailableUnitsResponse
            {
                Units = item.Units,
                ContractId = item.ContractId,
                IsPack = item.IsPack,
                type = (int)item.type,
                OwnerId = item.OwnerId,
            };
        }

        public static UpsertContractCommand ToModel(this ContractCreateRequest item)
        {
            var response = new UpsertContractCommand();
            response = item.ToModel();
            return response;
        }

        public static UpdateSalesforceIdsRequest ToDomain(this UpdateContractProductSForceId item)
        {
            return new UpdateSalesforceIdsRequest
            {
                ContractSalesForceId = item.ContractSalesforceId ?? string.Empty,
                ContractId = item.ContractId,
                ContractProductSalesforceIds = item.ContractProductSalesforceIds.Select(c => c.ToDomain()).ToList()
            };
        }

        public static ContractProductSFInfo ToDomain(this ContractProdSForceId item)
        {
            return new ContractProductSFInfo
            {
                ProductId = item.ProductId,
                SalesforceId = item.SalesforceId,
            };
        }

        public static ContractProductResponse ToDomain(this ContractProduct item)
        {
            var response = new ContractProductResponse();
            response = _mapper.Map(item, response);
            return response;
        }

        public static ContractResponse ToDomain(this Contract item)
        {
            var response = new ContractResponse();
            response = _mapper.Map(item, response);
            return response;
        }

        public static ProductLineResponse ToDomain(this ProductLine item)
        {
            var response = new ProductLineResponse();
            response = _mapper.Map(item, response);
            return response;
        }

        public static RegEnterpriseContractResponse ToDomain(this RegEnterpriseContract item)
        {
            var response = new RegEnterpriseContractResponse();
            response = _mapper.Map(item, response);
            return response;
        }

        public static RegEnterpriseConsumResponse ToDomain(this RegEnterpriseConsum item)
        {
            var response = new RegEnterpriseConsumResponse();
            response = _mapper.Map(item, response);
            return response;
        }

        public static ContractCreationResponse ToCommand(this ContractCreationDomainResponse item)
        {
            var response = new ContractCreationResponse
            {
                ContractProducts = item.ContractProducts.Select(cp => cp.ToDomain()).ToList(),
                ProductLines = item.ProductLines.Select(pl => pl.ToDomain()).ToList(),
                Contract = item.Contract.ToDomain(),
                RegEnterpriseContracts = item.RegEnterpriseContracts.Select(regc => regc.ToDomain()).ToList(),
                RegEnterpriseConsums = item.RegEnterpriseConsums.Select(regco => regco.ToDomain()).ToList()
            };
            return response;
        }
    }
}
