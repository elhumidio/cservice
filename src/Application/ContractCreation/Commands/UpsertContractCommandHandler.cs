using API.DataContext;
using Application.ContractCreation.Dto;
using Application.Core;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;


namespace Application.ContractCreation.Commands
{
    public class CreateContractCommandHandler : IRequestHandler<UpsertContractCommand, Result<ContractCreationDomainResponse>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private const int WELCOME_PRODUCT = 110;

        public CreateContractCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            uow = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<Result<ContractCreationDomainResponse>> Handle(UpsertContractCommand request, CancellationToken cancellationToken)
        {
            try
            {

                //var salesforceTransaction = new SalesforceTransaction
                //{
                //    StartDate = DateTime.Now,
                //    Success = false,
                //    TurijobsId = contractId,
                //    ObjectTypeName = "Opportunity",
                //    FinishDate = DateTime.Now.Date,
                //    Idsite = request.IDSite,
                //};
                //  var transId = uow.SalesforceTransRepository.Add(salesforceTransaction);
                ContractCreationDomainResponse response = new();
                var company = uow.EnterpriseRepository.Get(request.IDEnterprise);

                //Apply restriction region
                foreach (var prodId in request.ProductsList)
                {
                    var product = uow.ProductRepository.Get(prodId).FirstOrDefault(p => p.Idsite == request.IDSite);
                    var finishDate = DateTime.Now.AddDays(product?.Duration ?? 0);
                    response.Contract = await CreateContract(finishDate, request, company);

                    if (response.Contract == null)
                    {
                        return Result<ContractCreationDomainResponse>.Failure("Couldn't create contract");
                    }
                    //Añadir logica de calculo de fecha segun los productos
                    //var product = uow.ProductRepository.Get(110);
                    var productLines = uow.ProductLineRepository.GetProductLinesByProductId(prodId)
                        .Where(pl => pl.Idsite == company.SiteId && (product.ChkService ? pl.IdjobVacType == null : pl.IdjobVacType != null
                        && pl.Idslanguage == request.IDSLanguage))
                        .ToList();

                    /*var productLines1 = uow.ProductLineRepository
                        .GetProductLinesByProductId(prodId).Where(pl => pl.Idsite == company.SiteId && pl.Idslanguage == request.IDSLanguage)
                        .ToList();*/

                    response.ProductLines = productLines;
                    foreach (var pl in productLines)
                    {
                        var cp = new ContractProduct
                        {
                            Idproduct = prodId,
                            Idcontract = response.Contract.Idcontract,
                            Price = product.Price,
                            Units = pl.Units
                        };
                        mapper.Map(pl, cp);
                        cp.Idcontract = response.Contract.Idcontract;
               
                  
                  
                        var valueId = await uow.ContractProductRepository.CreateContractProduct(cp);
                        response.ContractProducts.Add(cp);
                        try {
                            uow.Commit();
                        }
                        catch (Exception ) {

                            uow.Rollback();

                        }
                        
                        if (valueId < 1)
                        {
                            return Result<ContractCreationDomainResponse>.Failure("Couldn't create contract");
                        }


                        if (request.IDSite == (int)Sites.PORTUGAL && product.ChkService)
                        {
                            response.RegEnterpriseConsums.Add(await SaveRegEnterPriseConsums(request, product, response.Contract.Idcontract));
                        }
                        if (!product.ChkService)
                        {
                            response.RegEnterpriseContracts.Add(await SaveRegEnterpriseContract(request, product, response.Contract.Idcontract, pl));
                        }

                        var enterpriseUserJobVac = CreateUserJobVac(response, product);                        
                        await uow.EnterpriseUserJobVacRepository.Add(enterpriseUserJobVac);

                        var cpr = CreateRegionRestrictionObject(response, company, prodId);
                        await uow.ContractPublicationRegionRepository.AddRestriction(cpr);
                    }
                }
                try
                {
                    uow.Commit();
                }
                catch (Exception) {
                    //TODO rollback de contrato 
                    uow.Rollback();
                }
                
                // TODO: Update salesforceTransaction

                return Result<ContractCreationDomainResponse>.Success(response);
            }
            catch (Exception ex)
            {
                uow.Rollback();
                return Result<ContractCreationDomainResponse>.Failure(ex.Message);
            }
        }

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

        private EnterpriseUserJobVac CreateUserJobVac(ContractCreationDomainResponse response, Product? product)
        {
            EnterpriseUserJobVac enterpriseUserJobVac = new();
            mapper.Map(response.Contract, enterpriseUserJobVac);
            mapper.Map(response.ProductLines.First(), enterpriseUserJobVac);
            mapper.Map(product, enterpriseUserJobVac);
            enterpriseUserJobVac.JobVacUsed = enterpriseUserJobVac.MaxJobVacancies;
            return enterpriseUserJobVac;
        }

        private async Task<Contract> CreateContract(DateTime finishDate, UpsertContractCommand request, Enterprise company)
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

        private async Task<RegEnterpriseConsum> SaveRegEnterPriseConsums(UpsertContractCommand request, Domain.Entities.Product? product, int contractId)
        {
            var regConsums = new RegEnterpriseConsum
            {
                Identerprise = request.IDEnterprise,//cvs
                UnitsUsed = 0,
                Idcontract = contractId,
                Units = 200
            };
            await uow.RegEnterpriseConsumsRepository.Add(regConsums);
            return regConsums;
        }

        private async Task<RegEnterpriseContract> SaveRegEnterpriseContract(UpsertContractCommand request, Domain.Entities.Product? product, int contractId, ProductLine pl)
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
    }
}
