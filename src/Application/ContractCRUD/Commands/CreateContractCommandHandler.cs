using API.DataContext;
using Application.ContractCreation.Dto;
using Application.Contracts.Queries;
using Application.Core;
using AutoMapper;
using Domain.DTO.Products;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Application.ContractCRUD.Commands
{
    public class CreateContractCommandHandler : IRequestHandler<CreateContractCommand, Result<ContractCreationDomainResponse>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private const int WELCOME_PRODUCT = 110;
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;

        public CreateContractCommandHandler(IUnitOfWork _unitOfWork,
            IMapper _mapper,
            IMediator mediator,
            IProductRepository productRepository)
        {
            uow = _unitOfWork;
            mapper = _mapper;
            _mediator = mediator;
            _productRepository = productRepository;
        }

        public async Task<Result<ContractCreationDomainResponse>> Handle(CreateContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ContractCreationDomainResponse response = new();
                var company = uow.EnterpriseRepository.Get(request.IDEnterprise);
                var finishDate = GetContractDurationByProducts(request.ProductsList);
                response.Contract = await CreateContract(finishDate, request, company);
                foreach (var prod in request.ProductsList)
                {                    
                    var prodObj = _productRepository.Get(prod.Idproduct).FirstOrDefault();
                    if (response.Contract == null)
                    {
                        return Result<ContractCreationDomainResponse>.Failure("Couldn't create contract");
                    }
                    var productLines = uow.ProductLineRepository.GetProductLinesByProductId(prod.Idproduct)
                        .Where(pl =>  (prodObj.ChkService ? pl.IdjobVacType == null : pl.IdjobVacType != null
                        && pl.Idslanguage == request.IDSLanguage)).GroupBy(g => g.Idproduct)
                        .ToList();

                    response.ProductLines.AddRange(uow.ProductLineRepository.GetProductLinesByProductId(prod.Idproduct)
                        .Where(pl => pl.Idsite == company.SiteId && (prodObj.ChkService ? pl.IdjobVacType == null : pl.IdjobVacType != null
                        && pl.Idslanguage == request.IDSLanguage)).ToList());
                    foreach (var pl in productLines)
                    {
                        int price = 0;
                        var priceObj = await _productRepository.GetPriceByProductIdAndCountryId(pl.Key, request.CountryId);
                        if(priceObj == null)
                        {
                            priceObj = await _productRepository.GetPriceByProductIdAndCountryId(pl.Key, (int)CountriesTurijobsDefined.Spain);
                        }
                        
                        var cp = new ContractProduct
                        {
                            Idproduct = prod.Idproduct,
                            Idcontract = response.Contract.Idcontract,
                            Price = priceObj?.Price ?? 0,
                            Units = request.ProductsList.Where(p => p.Idproduct == pl.Key).First().Units
                        };
                        mapper.Map(pl.First(), cp);
                        cp.Idcontract = response.Contract.Idcontract;

                        var valueId = await uow.ContractProductRepository.CreateContractProduct(cp);
                        response.ContractProducts.Add(cp);
                        try
                        {
                            uow.Commit();
                        }
                        catch (Exception)
                        {
                            uow.Rollback();
                        }

                        if (valueId < 1)
                        {
                            return Result<ContractCreationDomainResponse>.Failure("Couldn't create contract");
                        }

                        if (prodObj.ChkService)
                        {
                            response.RegEnterpriseConsums.Add(await SaveRegEnterPriseConsums(request, prodObj, response.Contract.Idcontract));
                        }
                        if (!prodObj.ChkService)
                        {
                            response.RegEnterpriseContracts.Add(await SaveRegEnterpriseContract(request, prodObj, response.Contract.Idcontract, pl.First()));
                            var enterpriseUserJobVac = CreateUserJobVac(response, prodObj);
                            await uow.EnterpriseUserJobVacRepository.Add(enterpriseUserJobVac);
                        }

                        var products = await _mediator.Send(new GetAllProductsByContract.GetProducts
                        {
                            ContractId = response.Contract.Idcontract,
                            LanguageID = request.IDSLanguage,
                            SiteId = request.IDSite
                        });

                        response.ProductsDescriptions = products.Value;
                    }
                }
                try
                {
                    uow.Commit();
                }
                catch (Exception)
                {
                    uow.Rollback();
                }
                return Result<ContractCreationDomainResponse>.Success(response);
            }
            catch (Exception ex)
            {
                uow.Rollback();
                return Result<ContractCreationDomainResponse>.Failure(ex.Message);
            }
        }

        #region PRIVATE METHODS

        private static ContractPublicationRegion CreateRegionRestrictionObject(ContractCreationDomainResponse response, Enterprise company, int prodId)
        {
            return new()
            {
                CreationDate = DateTime.Now,
                DeactivationDate = DateTime.Now.AddDays(365),
                ChkActive = true,
                Idcontract = response.Contract.Idcontract,
                Idproduct = prodId,
                Idregion = company.Idregion,
                Idsite = company.SiteId ?? (int)Sites.SPAIN,
                DeactivationBouserId = string.Empty,
                CreationBouserId = "backend",
            };
        }

        private DateTime GetContractDurationByProducts(List<ProductUnits> prods)
        {
            DateTime calculatedDate = DateTime.Now;

            foreach (var prod in prods)
            {
               var dateByProd = DateTime.Now.AddDays(_productRepository.Get(prod.Idproduct).FirstOrDefault().Duration);
               if(dateByProd > calculatedDate)
                {
                    calculatedDate = dateByProd;
                }
            }

            return calculatedDate;


        }

        private EnterpriseUserJobVac CreateUserJobVac(ContractCreationDomainResponse response, Product? product)
        {
            EnterpriseUserJobVac enterpriseUserJobVac = new();
            mapper.Map(response.Contract, enterpriseUserJobVac);
            mapper.Map(response.ProductLines.First(), enterpriseUserJobVac);
            mapper.Map(product, enterpriseUserJobVac);
            enterpriseUserJobVac.JobVacUsed = enterpriseUserJobVac.MaxJobVacancies;
            return enterpriseUserJobVac;
        }

        private async Task<Contract> CreateContract(DateTime finishDate, CreateContractCommand request, Enterprise company)
        {
            Contract con = new();
            con = mapper.Map(request, con);
            con = mapper.Map(company, con);
            con.StartDate = DateTime.Now.Date;
            con.FinishDate = finishDate;
            con.ApprovedDate = DateTime.Now.Date;
            con.ContractDate = DateTime.Now.Date;
            con.Sftimestamp = DateTime.Now.Date;
            con.ChkApproved = true;
            var contractId = await uow.ContractRepository.CreateContract(con);
            return con;
        }

        private async Task<RegEnterpriseConsum> SaveRegEnterPriseConsums(CreateContractCommand request, Product? product, int contractId)
        {
            var regConsums = new RegEnterpriseConsum
            {
                Identerprise = request.IDEnterprise,
                UnitsUsed = 0,
                Idcontract = contractId,
                Units = request.ProductsList.Where(a => a.Idproduct == product.Idproduct).FirstOrDefault().Units
            };
            await uow.RegEnterpriseConsumsRepository.Add(regConsums);
            return regConsums;
        }

        private async Task<RegEnterpriseContract> SaveRegEnterpriseContract(CreateContractCommand request, Product? product, int contractId, ProductLine pl)
        {
            var regContract = new RegEnterpriseContract();
            mapper.Map(request, regContract);
            mapper.Map(product, regContract);
            regContract.Idcontract = contractId;
            regContract.Units = pl.Units;
            regContract.IdjobVacType = pl.IdjobVacType ?? 0;
            var idreg = await uow.RegEnterpriseContractRepository.Add(regContract);
            return regContract;
        }

        #endregion PRIVATE METHODS
    }
}
