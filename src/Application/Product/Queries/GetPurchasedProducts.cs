using Application.Core;
using AutoMapper;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.Product.Queries
{
    public class GetPurchasedProducts
    {
        public class GetProducts : IRequest<Result<List<ProductsPurchasedDto>>>
        {
            public int CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<GetProducts, Result<List<ProductsPurchasedDto>>>
        {
            private readonly IContractRepository _contractRepo;

            public Handler(IMapper mapper, IContractRepository contractRepo)
            {
                _contractRepo = contractRepo;
            }

            public async Task<Result<List<ProductsPurchasedDto>>> Handle(GetProducts request, CancellationToken cancellationToken)
            {
                var products = await _contractRepo.GetPurchasedProductsByCompany(request.CompanyId);
                return Result<List<ProductsPurchasedDto>>.Success(products);
            }
        }
    }
}
