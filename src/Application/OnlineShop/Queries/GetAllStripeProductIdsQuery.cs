using Application.Core;
using Domain.DTO.Products;
using Domain.Repositories;
using MediatR;

namespace Application.OnlineShop.Queries
{
    public class GetAllStripeProductIdsQuery : IRequest<List<ProductsPricesByQuantityAndCountryDto>>
    {
    }

    public class GetAllStripeProductIdsHandler : IRequestHandler<GetAllStripeProductIdsQuery, List<ProductsPricesByQuantityAndCountryDto>>
    {

        private readonly IProductRepository productRepository;

        public GetAllStripeProductIdsHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public Task<List<ProductsPricesByQuantityAndCountryDto>> Handle(GetAllStripeProductIdsQuery request, CancellationToken cancellationToken)
        {
            return productRepository.GetAllStripeProductIds(); ;
        }
    }
}
