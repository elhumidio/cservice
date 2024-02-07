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
                decimal totalPrice = 0;
                int? pricePartial = 0;

                List<ProductUnits> list = new List<ProductUnits>();
                foreach (var product in request.ProductsList)
                {
                    ProductUnits obj = new()
                    {
                        Idproduct = product.Idproduct,
                        Units = product.Units
                    };
                    list.Add(obj);
                }

                var prices = await _productRepository.GetPricesByQuantityAndCountry(list, request.CountryId);

                if (request.IDContract <= 0)
                {

                    response.Contract = await CreateContract(finishDate, request, company, totalPrice);
                }
                else
                {
                    //get contract by id
                    response.Contract = uow.ContractRepository.Get(request.IDContract).FirstOrDefault();
                }

                foreach (var prod in request.ProductsList)
                {
                    var prodObj = _productRepository.Get(prod.Idproduct).FirstOrDefault();
                    if (response.Contract == null)
                    {
                        return Result<ContractCreationDomainResponse>.Failure("Couldn't create contract");
                    }
                    var productLines = uow.ProductLineRepository.GetProductLinesByProductId(prod.Idproduct)
                        .Where(pl => (prodObj.ChkService ? pl.IdjobVacType == null : pl.IdjobVacType != null
                        && pl.Idslanguage == request.IDSLanguage)).GroupBy(g => g.Idproduct)
                        .ToList();
                    response.ProductLines.AddRange(uow.ProductLineRepository.GetProductLinesByProductId(prod.Idproduct)
                        .Where(pl => pl.Idsite == company.SiteId && (prodObj.ChkService ? pl.IdjobVacType == null : pl.IdjobVacType != null
                        && pl.Idslanguage == request.IDSLanguage)).ToList());

                    foreach (var pl in productLines)
                    {
                        int price = 0;
                        var cp = new ContractProduct
                        {
                            Idproduct = prod.Idproduct,
                            Idcontract = response.Contract.Idcontract,
                            Price = prices.FirstOrDefault(p => p.ProductId == pl.Key).UnitPriceAfterDiscount,   // priceObj?.Price ?? 0,
                            Units = request.ProductsList.Where(p => p.Idproduct == pl.Key).First().Units
                        };
                        var plineToMap = request.ProductsList.Where(p => p.Idproduct == pl.Key).First();
                        cp.Units = plineToMap.Units;
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
                            //Get active users by enterprise
                            var enterpriseUsers = uow.EnterpriseUserRepository.GetCompanyAdmins(request.IDEnterprise,true);
                            foreach (var enterpriseUser in enterpriseUsers)
                            {
                                var evac = CreateUserJobVac(request, response, prodObj, pl.First(),enterpriseUser.IsAdmin);
                                evac.IdenterpriseUser = enterpriseUser.UserId;
                                await uow.EnterpriseUserJobVacRepository.Add(evac);
                            }
                            //   var enterpriseUserJobVac = CreateUserJobVac(request, response, prodObj, pl.First());
                            // await uow.EnterpriseUserJobVacRepository.Add(enterpriseUserJobVac);
                            uow.Commit();
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
            var calculatedDate = uow.ContractRepository.GetContractFinishDate(prods);
            return calculatedDate.Date;
        }

        private EnterpriseUserJobVac CreateUserJobVac(CreateContractCommand request, ContractCreationDomainResponse response, Product? product, ProductLine pl, bool isAdmin)
        {
            EnterpriseUserJobVac enterpriseUserJobVac = new();
            mapper.Map(response.Contract, enterpriseUserJobVac);
            enterpriseUserJobVac.Idproduct = pl.Idproduct;
            mapper.Map(pl, enterpriseUserJobVac);
            mapper.Map(product, enterpriseUserJobVac);
            enterpriseUserJobVac.IdjobVacType = pl.IdjobVacType ?? 0;
      
            enterpriseUserJobVac.IdenterpriseUser = request.IDEnterpriseUSer;
            var productInRequest = request.ProductsList.FirstOrDefault(a => a.Idproduct == product.Idproduct);
            if (productInRequest != null)
            {
                enterpriseUserJobVac.MaxJobVacancies = productInRequest.Units;
                enterpriseUserJobVac.JobVacUsed = enterpriseUserJobVac.MaxJobVacancies;
            }
            if (!isAdmin)
            {
                enterpriseUserJobVac.JobVacUsed = 0;
            }
            return enterpriseUserJobVac;
        }

        private async Task<Contract> CreateContract(DateTime finishDate, CreateContractCommand request, Enterprise company, decimal price)
        {
            Contract con = new();
            con = mapper.Map(request, con);
            con = mapper.Map(company, con);
            con.Idcontract = 0;
            con.StartDate = DateTime.Now.Date;
            con.FinishDate = finishDate;
            con.ApprovedDate = DateTime.Now.Date;
            con.ContractDate = DateTime.Now.Date;
            con.Sftimestamp = DateTime.Now.Date;
            con.ChkApproved = true;
            con.Price = price;
            con.FinalPrice = price;
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
                Units = request.ProductsList.Where(a => a.Idproduct == product.Idproduct).FirstOrDefault().Units,
                Idproduct = product.Idproduct
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
            regContract.Units = request.ProductsList.Where(p => p.Idproduct == product.Idproduct).First().Units;
            regContract.IdjobVacType = pl.IdjobVacType ?? 0;
            var idreg = await uow.RegEnterpriseContractRepository.Add(regContract);
            return regContract;
        }

        #endregion PRIVATE METHODS
    }
}
