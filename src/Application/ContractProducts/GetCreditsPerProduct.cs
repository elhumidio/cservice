using Application.ContractProducts.DTO;
using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.ContractProducts
{
    public class GetCreditsPerProduct
    {

        public class Query : IRequest<Result<List<CreditsPerProductDto>>>
        {
            public int EnterpriseId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<CreditsPerProductDto>>>
        {

            private readonly IUnitsRepository _unitsRepository;

            public Handler(IUnitsRepository unitsRepository)
            {
                _unitsRepository = unitsRepository;
            }

            public async Task<Result<List<CreditsPerProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<CreditsPerProductDto>>.Success(_unitsRepository.GetCreditsPerProduct(request.EnterpriseId));
            }
        }
    }
}
