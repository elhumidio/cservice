using Application.Core;
using AutoMapper;
using Domain.DTO.Products;
using Domain.Enums;
using Domain.Repositories;
using MediatR;

namespace Application.OnlineShop.Queries
{
    public class GetProductsAndPricesAndDiscountsCommand : IRequest<Result<List<ProductsPricesByQuantityAndCountryDto>>>
    {
        public List<ProductUnits> Products { get; set; }
        public int? CountryId { get; set; }
    }

    public class Handler : IRequestHandler<GetProductsAndPricesAndDiscountsCommand, Result<List<ProductsPricesByQuantityAndCountryDto>>>
    {
        private readonly IProductRepository _productsRepository;

        public Handler(IProductRepository productRepository)
        {
            _productsRepository = productRepository;
        }

        public async Task<Result<List<ProductsPricesByQuantityAndCountryDto>>> Handle(GetProductsAndPricesAndDiscountsCommand request, CancellationToken cancellationToken)
        {
            var query = await _productsRepository.GetPricesByQuantityAndCountry( request.Products, request.CountryId ?? (int)CountriesTurijobsDefined.Spain);
            query.ForEach(x => x.ProductName = _productsRepository.GetProductName(x.ProductId));
            return Result<List<ProductsPricesByQuantityAndCountryDto>>.Success(query);
        }
    }
}
