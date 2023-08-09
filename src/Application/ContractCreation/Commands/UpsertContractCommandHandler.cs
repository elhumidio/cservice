using Application.Core;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.ContractCreation.Commands
{
    public class UpsertContractCommandHandler : IRequestHandler<UpsertContractCommand, Result<int>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public UpsertContractCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            uow = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<Result<int>> Handle(UpsertContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var company = uow.EnterpriseRepository.Get(request.IDEnterprise);
                var contract = mapper.Map<Contract>(company);
                var contractId = await uow.ContractRepository.CreateContract(contract);

                var salesforceTransaction = new SalesforceTransaction
                {
                    StartDate = DateTime.Now,
                    Success = false,
                    TurijobsId = contractId,
                    ObjectTypeName = "Opportunity"
                };
                var transId = uow.SalesforceTransRepository.Add(salesforceTransaction);

                foreach (var prodId in request.ProductsList)
                {
                    var product = uow.ProductRepository.Get(prodId);
                    var productLines = uow.ProductLineRepository.GetProductLinesByProductId(prodId)
                        .Where(pl => pl.Idsite == company.SiteId && (product.ChkService ? pl.IdjobVacType == null : pl.IdjobVacType != null))
                        .ToList();

                    foreach (var pl in productLines)
                    {
                        var cp = new ContractProduct
                        {
                            Idproduct = prodId,
                            Idcontract = contractId,
                            Price = product.Price
                        };
                        mapper.Map(pl, cp);

                        var valueId = await uow.ContractProductRepository.CreateContractProduct(cp);

                        if (!product.ChkService)
                        {
                            var regContract = new RegEnterpriseContract();
                            var idreg = await uow.RegEnterpriseContractRepository.Add(regContract);
                        }
                        else
                        {
                            var regConsums = new RegEnterpriseConsum();
                            var idreg = await uow.RegEnterpriseConsumsRepository.Add(regConsums);
                        }
                    }
                }

                uow.Commit();

                // TODO: Salesforce operations here

                // TODO: Update salesforceTransaction

                return Result<int>.Success(contractId);
            }
            catch (Exception ex)
            {
                uow.Rollback();
                return Result<int>.Failure(ex.Message);
            }
        }

    }
}
