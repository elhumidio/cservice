using API.DataContext;
using Application.ContractCreation.Dto;
using Application.Core;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.ContractCreation.Commands
{
    public class UpsertContractCommandHandler : IRequestHandler<UpsertContractCommand, Result<ContractCreationResponse>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private const int WELCOME_PRODUCT = 110;

        public UpsertContractCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            uow = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<Result<ContractCreationResponse>> Handle(UpsertContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ContractCreationResponse response = new ContractCreationResponse();
                var company = uow.EnterpriseRepository.Get(request.IDEnterprise);

                var product = uow.ProductRepository.Get(WELCOME_PRODUCT).FirstOrDefault(p => p.Idsite == request.IDSite);
                var finishDate = DateTime.Now.AddDays(product?.Duration ?? 0);
                response.Contract = await CreateContract(finishDate, request, company);

                if (response.Contract.Idcontract < 1)
                {
                    return Result<ContractCreationResponse>.Failure("Couldn't create contract");
                }

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

                //var product = uow.ProductRepository.Get(110);
                var productLines = uow.ProductLineRepository.GetProductLinesByProductId(WELCOME_PRODUCT)
                    .Where(pl => pl.Idsite == company.SiteId && (product.ChkService ? pl.IdjobVacType == null : pl.IdjobVacType != null
                    && pl.Idslanguage == request.IDSLanguage))
                    .ToList();
                response.ProductLines = productLines;
                foreach (var pl in productLines)
                {
                    var cp = new ContractProduct
                    {
                        Idproduct = WELCOME_PRODUCT,
                        Idcontract = response.Contract.Idcontract,
                        Price = product.Price,
                        Units = pl.Units
                    };
                    response.ContractProducts.Add(cp);
                    mapper.Map(pl, cp);
                    cp.Idcontract = response.Contract.Idcontract;
                    var valueId = await uow.ContractProductRepository.CreateContractProduct(cp);
                    uow.Commit();

                    if (request.IDSite == (int)Sites.PORTUGAL)
                    {
                        response.RegEnterpriseConsums.Add(await SaveRegEnterPriseConsums(request, product, response.Contract.Idcontract));
                    }
                    if (!product.ChkService)
                    {
                        response.RegEnterpriseContracts.Add(await SaveRegEnterpriseContract(request, product, response.Contract.Idcontract, pl));
                    }

                    EnterpriseUserJobVac enterpriseUserJobVac = new EnterpriseUserJobVac();
                    mapper.Map(response.Contract, enterpriseUserJobVac);
                    mapper.Map(response.ProductLines.First(), enterpriseUserJobVac);
                    mapper.Map(product, enterpriseUserJobVac);

                    await uow.EnterpriseUserJobVacRepository.Add(enterpriseUserJobVac);
                }

                uow.Commit();

                // TODO: Salesforce operations here

                // TODO: Update salesforceTransaction

                return Result<ContractCreationResponse>.Success(response);
            }
            catch (Exception ex)
            {
                uow.Rollback();
                return Result<ContractCreationResponse>.Failure(ex.Message);
            }
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
            var regConsums = new RegEnterpriseConsum();
            regConsums.Identerprise = request.IDEnterprise;//cvs
            regConsums.UnitsUsed = 0;
            regConsums.Idcontract = contractId;
            regConsums.Units = 200;
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
            var idreg = await uow.RegEnterpriseContractRepository.Add(regContract);
            return regContract;
        }
    }
}
