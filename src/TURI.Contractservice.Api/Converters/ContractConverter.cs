using Application.ContractCreation.Dto;
using Application.ContractCRUD.Commands;
using Application.ContractCRUD.Commands.Salesforce;
using Application.Contracts.DTO;
using AutoMapper;
using Domain.DTO;
using Domain.DTO.Products;
using Domain.DTO.Requests;
using Domain.Entities;
using TURI.ContractService.Contract.Models;
using TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder;
using TURI.ContractService.Contracts.Contract.Models.Partials;
using TURI.ContractService.Contracts.Contract.Models.Requests;
using TURI.ContractService.Contracts.Contract.Models.Response;

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
                cfg.CreateMap<CreateContractCommand, ContractCreateRequest>();
                cfg.CreateMap<ContractCreateRequest, CreateContractCommand>();
                cfg.CreateMap<ContractProductShortDto, ContractProductShortDtoResponse>();
                cfg.CreateMap<ContractProductSalesforceIdRequest, ContractProductSalesforceId>();
                cfg.CreateMap<UpdateContract, UpdateContractCommand>();
                cfg.CreateMap<ProductUnitsRequest, ProductUnits>();
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

        public static CreateContractCommand ToDomain(this ContractCreateRequest item)
        {
            var response = new CreateContractCommand();
            response = _mapper.Map(item, response);
            return response;
        }

        public static UpdateContractCommand ToDomain(this UpdateContract item)
        {
            var response = new UpdateContractCommand();
            response = _mapper.Map(item, response);
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

        public static ContractProductShortDtoResponse ToDomain(this ContractProductShortDto item)
        {
            var response = new ContractProductShortDtoResponse();
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
                RegEnterpriseConsums = item.RegEnterpriseConsums.Select(regco => regco.ToDomain()).ToList(),
                contractProductShortDtoResponses = item.ProductsDescriptions.Select(v => v.ToDomain()).ToList()
            };
            return response;
        }

        public static ContractProductSalesforceId ToModel(this ContractProductSalesforceIdRequest item)
        {
            var response = new ContractProductSalesforceId();
            response = _mapper.Map(item, response);
            return response;
        }

        public static UpdateContractProductSalesforceIdRequest ToCommand(this WrapperContractProductSalesforceIdRequest item)
        {
            var response = new UpdateContractProductSalesforceIdRequest();
            response.ContractSalesForceId = item.ContractSalesForceId;
            response.ContractId = item.ContractId;
            response.ContractProductSalesforceIds = item.ContractProductSalesforceIds.Select(c => c.ToModel()).ToList();
            return response;
        }

        public static KeyValuesResponse ToResponse(this KeyValueResponse item)
        {
            return new KeyValuesResponse
            {
                Id = item.Id,
                Value = item.Value,
            };
        }
    }
}
