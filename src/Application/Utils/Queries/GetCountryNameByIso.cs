using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.Utils.Queries
{
    public class GetCountryNameByIso
    {
        public class Get : IRequest<Result<string>>
        {
            public string IsoCode { get; set; }
       
        }

        public class Handler : IRequestHandler<Get, Result<string>>
        {
            private  ICountryIsoRepository _countryIsoRepo { get; set; }

            public Handler(ICountryIsoRepository countryIsoRepository)
            {
                _countryIsoRepo = countryIsoRepository;
            }

            public async Task<Result<string>> Handle(Get request, CancellationToken cancellationToken)
            {
                return Result<string>.Success(_countryIsoRepo.CountryNameByIso(request.IsoCode));
            }
        }
    }
}
