using Application.Core;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Utils.Queries
{
    public class GetZipCodeFromCityName
    {
        public class Get : IRequest<Result<ZipCode>>
        {
            public string CityName { get; set;}
            public Get(string cityName)
            {
                CityName = cityName;
            }
        }

        public class Handler : IRequestHandler<Get, Result<ZipCode>>
        {
            public IZipCodeRepository _zipCodeRepository { get; set; }

            public Handler(IZipCodeRepository zipCodeRepository)
            {
                _zipCodeRepository = zipCodeRepository;
            }

            public async Task<Result<ZipCode>> Handle(Get request, CancellationToken cancellationToken)
            {
                return Result<ZipCode>.Success(_zipCodeRepository.GetZipCodeByCityName(request.CityName));
            }
        }
    }
}
