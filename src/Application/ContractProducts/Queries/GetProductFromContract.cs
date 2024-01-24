using Application.Contracts.DTO;
using Application.Core;
using AutoMapper;
using Domain.DTO.Products;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ContractProducts.Queries
{
    public class GetProductFromContract
    {
        public class Query : IRequest<Result<List<ProductUnits>>>
        {
            public int ContractID { get; set; }
            
        }

        public class Handler : IRequestHandler<Query, Result<List<ProductUnits>>>
        {
            private readonly IContractProductRepository _contProdRepo;            

            public Handler(IContractProductRepository contractProductRepository)
            {
                 _contProdRepo = contractProductRepository;
            }

            public async Task<Result<List<ProductUnits>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = _contProdRepo.GetProductsAndUnitsByContract(request.ContractID);
                 return Result<List<ProductUnits>>.Success(products);
            }

         
        }
    }
}
