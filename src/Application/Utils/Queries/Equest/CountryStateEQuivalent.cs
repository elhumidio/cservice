using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.Utils.Queries.Equest
{
    public class CountryStateEQuivalent
    {
        public class Query : IRequest<Result<int>>
        {
            public string countryId { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<int>>
        {
            ICountryStateEQRepository _countryEqRepo;
            public Handler(ICountryStateEQRepository countryEqRepo)
            {
                _countryEqRepo = countryEqRepo;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                var ret = await _countryEqRepo.GetCountryStateEQ(request.countryId);
                return Result<int>.Success(ret);
            }

        }
    }
}
