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

        public UpsertContractCommandHandler(IUnitOfWork _unitOfWork,IMapper _mapper)
        {
            uow = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<Result<int>> Handle(UpsertContractCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var company = uow.EnterpriseRepository.Get(request.IDEnterprise);
                Contract contract = new Contract();
                mapper.Map(company, contract);                
                var contractId = await uow.ContractRepository.CreateContract(contract);

                SalesforceTransaction salesforceTransaction = new SalesforceTransaction();
                salesforceTransaction.StartDate = DateTime.Now;
                salesforceTransaction.Success = false;
                salesforceTransaction.TurijobsId = contractId;
                salesforceTransaction.ObjectTypeName = "Opportunity";
                var transId = uow.SalesforceTransRepository.Add(salesforceTransaction);    

                //List<ContractProduct> cproducts = new();

                foreach (var prodId in request.ProductsList)
                {
                    var product = uow.ProductRepository.Get(prodId);
                    var productLines = uow.ProductLineRepository.GetProductLinesByProductId(prodId).ToList();
                    ContractProduct cp = new ContractProduct(); 

                    var valueId = await uow.ContractProductRepository.CreateContractProduct(cp);
                }
                EnterpriseUserJobVac enterpriseUserJobVac = new EnterpriseUserJobVac();

                var ret =  await uow.EnterpriseUserJobVacRepository.Add(enterpriseUserJobVac);

                RegEnterpriseContract regContract = new RegEnterpriseContract();
                var idreg = await uow.RegEnterpriseContractRepository.Add(regContract);
                RegEnterpriseConsum regEnterpriseContract = new RegEnterpriseConsum();
                var idConsums = await uow.RegEnterpriseConsumsRepository.Add(regEnterpriseContract);

                uow.Commit();
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
