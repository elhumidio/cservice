using Application.Interfaces;
using Application.JobOffer.Commands;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class ZipCodeValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly IGeoNamesConector _geoNames;
        private readonly ICountryIsoRepository _countryIsoRepo;
        private readonly IZipCodeRepository _zipCodeRepo;
        private readonly IRegionRepository _regionRepo;

        public ZipCodeValidator(IGeoNamesConector geoNames, ICountryIsoRepository countryIsoRepo, IZipCodeRepository zipCodeRepo, IRegionRepository regionRepo)
        {
            _geoNames = geoNames;
            _countryIsoRepo = countryIsoRepo;
            _zipCodeRepo = zipCodeRepo;
            _regionRepo = regionRepo;

            RuleFor(command => command).Must(IsRightPostalCode)
                .WithMessage("ZipCode is wrong formatted");
        }

        private bool IsRightPostalCode(CreateOfferCommand obj)
        {
            var ret = false;
            var countryCode = GetCountryIsoByIdCountry(obj.Idcountry);
            if (!string.IsNullOrEmpty(countryCode))
            {
                var ans = _geoNames.GetPostalCodesCollection(obj.ZipCode, GetCountryIsoByIdCountry(obj.Idcountry));
                if (ans != null && ans.postalCodes.Any())
                {
                    var IdzipCode = _zipCodeRepo.GetZipCodeIdByCodeAndCountry(obj.ZipCode, obj.Idcountry);
                    if (IdzipCode != 0)
                        obj.IdzipCode = IdzipCode;
                    else if (IdzipCode == 0)
                    {
                        //go for another id based on location name
                        IdzipCode = _zipCodeRepo.GetZipCodeIdByCity(obj.City);
                        if (IdzipCode != 0)
                            obj.IdzipCode = IdzipCode;
                        if (IdzipCode == 0)
                        {
                        
                            obj.Idcity = _zipCodeRepo.GetCityIdByName(ans.postalCodes.First().placeName);

                            ZipCode zip = new ZipCode
                            {
                                Zip = ans.postalCodes.First().postalCode,
                                Idcountry = obj.Idcountry,
                                Idregion = obj.Idregion,
                                Idcity = obj.Idcity,
                                Longitude = Convert.ToDecimal(ans.postalCodes.First().lng),
                                Latitude = Convert.ToDecimal(ans.postalCodes.First().lat),
                                City = ans.postalCodes.First().placeName
                            };
                            _zipCodeRepo.Add(zip);
                            obj.IdzipCode = 0;
                        }
                    }

                    ret = true;
                }
                else
                {
                    var zipCodeEntity = _zipCodeRepo.GetZipCodeEntity(obj.ZipCode, obj.Idcountry);
                    if (zipCodeEntity != null)
                    {
                        obj.IdzipCode = zipCodeEntity.IdzipCode;
                        var region = _regionRepo.Get(zipCodeEntity.Idregion);
                        obj.JobLocation = region != null ? region.BaseName : string.Empty;
                        ret = true;
                    }
                    else ret = false;
                }
            }
            else
            {
                ret = false;
            }

            return ret;
        }

        private string GetCountryIsoByIdCountry(int countryId)
        {
            var isoCode = _countryIsoRepo.GetIsobyCountryId(countryId);
            return isoCode;
        }
    }

    public class ZipCodeValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private readonly IGeoNamesConector _geoNames;
        private readonly ICountryIsoRepository _countryIsoRepo;
        private readonly IZipCodeRepository _zipCodeRepo;
        private readonly IRegionRepository _regionRepo;

        public ZipCodeValidatorUp(IGeoNamesConector geoNames, ICountryIsoRepository countryIsoRepo, IZipCodeRepository zipCodeRepo, IRegionRepository regionRepo)
        {
            _geoNames = geoNames;
            _countryIsoRepo = countryIsoRepo;
            _zipCodeRepo = zipCodeRepo;
            _regionRepo = regionRepo;

            RuleFor(command => command).Must(IsRightPostalCode)
                .WithMessage("ZipCode is wrong formatted");
        }

        private bool IsRightPostalCode(UpdateOfferCommand obj)
        {
            bool ret = false;
            var ans = _geoNames.GetPostalCodesCollection(obj.ZipCode, GetCountryIsoByIdCountry(obj.Idcountry));
            if (ans != null && ans.postalCodes.Any())
            {
                //update idzipcode in persistence object
                var IdzipCode = _zipCodeRepo.GetZipCodeIdByCodeAndCountry(obj.ZipCode, obj.Idcountry);
                if (IdzipCode != 0)
                    obj.IdzipCode = IdzipCode;
                else if (IdzipCode == 0)
                {
                    //go for another id based on location name
                    IdzipCode = _zipCodeRepo.GetZipCodeIdByCity(obj.City);
                    if (IdzipCode != 0)
                        obj.IdzipCode = IdzipCode;
                    if (IdzipCode == 0)
                    {
                        obj.IdzipCode = 0;
                    }
                }

                //Get cityId based on Postal code
                obj.Idcity = _zipCodeRepo.GetCityIdByZip(obj.ZipCode);

                ret = true;
            }
            else
            {
                var zipCodeEntity = _zipCodeRepo.GetZipCodeEntity(obj.ZipCode, obj.Idcountry);
                if (zipCodeEntity != null)
                {
                    obj.IdzipCode = zipCodeEntity.IdzipCode;
                    var region = _regionRepo.Get(zipCodeEntity.Idregion);
                    obj.JobLocation = region != null ? region.BaseName : string.Empty;
                    ret = true;
                }
                else ret = false;
            }
            return ret;
        }

        private string GetCountryIsoByIdCountry(int countryId)
        {
            var isoCode = _countryIsoRepo.GetIsobyCountryId(countryId);
            return isoCode;
        }
    }
}
