using Application.Core;
using Application.Interfaces;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.Utils.Queries
{
    public class GetIdsbyZipCode
    {
        public class GetIds : IRequest<Result<IdsDto>>
        {
            public string ZipCode { get; set; }
            public int CountryId { get; set; }
        }

        public class Handler : IRequestHandler<GetIds, Result<IdsDto>>
        {
            private readonly IZipCodeRepository _zipCodeRepository;
            private readonly IGeoNamesConector _geoNamesConector;
            private readonly ICountryIsoRepository _countryIsoRepository;
            private readonly IRegionRepository _regionRepo;
            private readonly ICityRepository _cityRepo;
            private readonly IZipCodeRepository _zipCodeRepo;
            private readonly IinternalService _internalService;

            public Handler(IZipCodeRepository zipCodeRepository,
                IGeoNamesConector geoNamesConector,
                ICountryIsoRepository countryIsoRepository,
                IRegionRepository regionRepository,
                ICityRepository cityRepository,
                IZipCodeRepository zipCodeRepo,
                IinternalService internalService)
            {
                _zipCodeRepository = zipCodeRepository;
                _geoNamesConector = geoNamesConector;
                _countryIsoRepository = countryIsoRepository;
                _regionRepo = regionRepository;
                _cityRepo = cityRepository;
                _zipCodeRepo = zipCodeRepo;
                _internalService = internalService;
            }

            public async Task<Result<IdsDto>> Handle(GetIds request, CancellationToken cancellationToken)
            {
                IdsDto dto = new();
                var zip = await _zipCodeRepository.GetZipCodeByZipAndCountry(request.ZipCode, request.CountryId);
                if (zip != null)
                {
                    dto.ZipCode = request.ZipCode;
                    dto.ZipCodeId = zip.IdzipCode;
                    dto.CountryId = request.CountryId;
                    dto.CityId = (int)zip.Idcity;
                    dto.City = zip.City;
                    dto.RegionId = zip.Idregion;
                }
                else
                {
                    var datacode = _geoNamesConector.GetPostalCodesCollection(request.ZipCode, _countryIsoRepository.GetIsobyCountryId(request.CountryId));

                    //Probably useful in the future...
                    /* if (datacode != null && datacode.postalCodes.Any()) {
                        var googleLocations = _internalService.GetGooglelocationByPlace();
                        var lastRegionId = _regionRepo.GetLastRegionId();
                        Region regionEnt = new()
                        {
                            Idregion = lastRegionId + 1,
                            Idcountry = request.CountryId,
                            Idsite = 6,
                            BaseName = datacode.postalCodes.First().adminName2,
                            Ccaa = datacode.postalCodes.First().adminName1,
                            ChkActive = 1,
                            NumVacancies = 0,
                            Threshold = 0,
                            Idslanguage = 7,
                        };
                        await _regionRepo.Add(regionEnt);
                        regionEnt.Idslanguage = 14;
                        await _regionRepo.Add(regionEnt);

                        City cityEnt = new City
                        {
                            Idregion = regionEnt.Idregion,
                            Idcountry = request.CountryId,

                            //buscar lo de google
                            GoogleId = datacode.postalCodes.First().adminName3,
                            Coordinates = request.GoogleLocationObject.Coordinates,
                            Cpro = null,
                            Dc = null,
                            Cmun = null,
                            Name = datacode.postalCodes.First().adminName3,
                        };
                        var cityId = await _cityRepo.Add(cityEnt);
                        ZipCode zipObj = new ZipCode
                        {
                            Idregion = regionEnt.Idregion,
                            Idcountry = request.CountryId,
                            Idcity = cityEnt.Idcity,
                            City = datacode.postalCodes.First().adminName3,
                            Latitude = Convert.ToDecimal(lat),
                            Longitude = Convert.ToDecimal(longitude),
                            Zip = request.GoogleLocationObject.ZipCode
                        };
                        var idzipcode =  _zipCodeRepo.Add(zip);
                    }*/
                }
                return Result<IdsDto>.Success(dto);
            }
        }
    }
}
