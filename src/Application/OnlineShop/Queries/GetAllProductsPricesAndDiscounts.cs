using Application.Core;
using Domain.DTO.Products;
using Domain.Repositories;
using MediatR;

namespace Application.OnlineShop.Queries
{
    public class GetAllProductsPricesAndDiscounts
    {
        public class GetAll : IRequest<Result<List<ProductsPricesByQuantityAndCountryDto>>>
        {
            public int CountryId { get; set; }
        }

        public class Handler : IRequestHandler<GetAll, Result<List<ProductsPricesByQuantityAndCountryDto>>>
        {
            private readonly IProductRepository _productRepository;

            public Handler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<Result<List<ProductsPricesByQuantityAndCountryDto>>> Handle(GetAll request, CancellationToken cancellationToken)
            {
                var values = await _productRepository.GetAllPricesByQuantityOrProduct(request.CountryId);
                if (values == null)
                {
                    return Result<List<ProductsPricesByQuantityAndCountryDto>>.Failure("Couldn't find any products");
                }
                return Result<List<ProductsPricesByQuantityAndCountryDto>>.Success(values);
            }
        }
    }
}
