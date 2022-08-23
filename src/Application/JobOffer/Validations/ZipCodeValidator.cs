using Application.Interfaces;
using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;


namespace Application.JobOffer.Validations
{
    public class ZipCodeValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly IGeoNamesConector _geoNames;
        private readonly ICountryIsoRepository _countryIsoRepo;
        private readonly IZipCodeRepository _zipCodeRepo;

        public ZipCodeValidator(IGeoNamesConector geoNames, ICountryIsoRepository countryIsoRepo, IZipCodeRepository zipCodeRepo)
        {
            _geoNames = geoNames;
            _countryIsoRepo = countryIsoRepo;
            _zipCodeRepo = zipCodeRepo;
            RuleFor(command => command).Must(IsRightPostalCode)
                .WithMessage("ZipCode is wrong formatted");
        }
        private bool IsRightPostalCode(CreateOfferCommand obj)
        {
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

                return true;
            }
            else return false;

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

        public ZipCodeValidatorUp(IGeoNamesConector geoNames, ICountryIsoRepository countryIsoRepo, IZipCodeRepository zipCodeRepo)
        {
            _geoNames = geoNames;
            _countryIsoRepo = countryIsoRepo;
            _zipCodeRepo = zipCodeRepo;
            RuleFor(command => command).Must(IsRightPostalCode)
                .WithMessage("ZipCode is wrong formatted");
        }
        private bool IsRightPostalCode(UpdateOfferCommand obj)
        {
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

                return true;
            }
            else return false;

        }

        private string GetCountryIsoByIdCountry(int countryId)
        {

            var isoCode = _countryIsoRepo.GetIsobyCountryId(countryId);
            return isoCode;
        }
    }
}
