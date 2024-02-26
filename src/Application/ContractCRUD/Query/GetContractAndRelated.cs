using Application.Core;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder;

namespace Application.ContractCRUD.Query
{
    public class GetContractAndRelated
    {
        public class Query : IRequest<Result<ContractCreationResponse>>
        {
            public int ContractId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContractCreationResponse>>
        {
            private readonly IContractRepository _contractRepo;
            private readonly IContractProductRepository _contractProduct;
            private readonly IRegEnterpriseContractRepository _regContractRepo;
            private readonly IEnterpriseUserJobVacRepository _entUserJobVacRepo;
            private readonly IProductLineRepository _productLineRepository;
            private readonly IProductRepository _productRepository;


            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IContractRepository contractRepo,
                IContractProductRepository contractProductRepository,
                IRegEnterpriseContractRepository regEnterpriseContractRepository,
                IEnterpriseUserJobVacRepository enterpriseUserJobVacRepository,
                IProductLineRepository productLineRepository,
                IProductRepository productRepository)
            {
                _mapper = mapper;
                _contractRepo = contractRepo;
                _contractProduct = contractProductRepository;
                _regContractRepo = regEnterpriseContractRepository;
                _entUserJobVacRepo = enterpriseUserJobVacRepository;
                _productLineRepository = productLineRepository;
                _productRepository = productRepository;
            }

            public async Task<Result<ContractCreationResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                ContractCreationResponse contract = new ContractCreationResponse();
                contract.Contract = new ContractResponse();
                var c = _contractRepo.GetById(request.ContractId);
                contract.Contract = _mapper.Map(c,contract.Contract);

                var contractProducts = _contractProduct.GetContractProducts(request.ContractId);
                contract.ContractProducts = _mapper.Map(contractProducts,contract.ContractProducts);

                var regContract = _regContractRepo.GetRegByContract(request.ContractId);
                contract.RegEnterpriseContracts = _mapper.Map(regContract,contract.RegEnterpriseContracts);

                foreach (var product in contractProducts)
                {
                    var pl = _productLineRepository.GetProductLinesByProductId(product.Idproduct).Where(p=> p.IdjobVacType != null).FirstOrDefault();
                    contract.ProductLines.Add(_mapper.Map(pl,new ProductLineResponse()));
                    ContractProductShortDtoResponse shortInfo = new()
                    {
                        ProductId = product.Idproduct,
                        ProductName = _productRepository.GetProductName(product.Idproduct)
                    };
                    contract.contractProductShortDtoResponses.Add(shortInfo);
                }
                return Result<ContractCreationResponse>.Success(contract);
            }
        }
    }
}
