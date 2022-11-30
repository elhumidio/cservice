using Application.JobOffer.Commands;
using Domain.Enums;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class CountryValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly ICountryRepository _countryRepo;
        private readonly IRegionRepository _regionRepo;


        public CountryValidator(ICountryRepository countryRepo, IRegionRepository regionRepo)
        {
            _countryRepo = countryRepo;
            _regionRepo = regionRepo;

            RuleFor(command => command.Idcountry)
                .Must(IsRightCountry)
                .WithMessage("Invalid value for Country field.\n")
                .NotNull()
                .WithMessage("Countryid is mandatory.\n");
            RuleFor(command => command).Must(IsCountryByRegion);
            
        }

        private bool IsRightCountry(int _countryId)
        {
            return _countryRepo.IsRightCountry(_countryId);
        }

        private bool IsCountryByRegion(CreateOfferCommand obj)
        {

            var country = _regionRepo.GetCountryByRegion(obj.Idregion);
            bool canSetCountry = country != -1 && obj.Idregion != (int)Regions.AllCountry && obj.Idregion != (int)Regions.Abroad;

            if (canSetCountry)
                obj.Idcountry = country;
            return true;

        }
    }

    public class CountryValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private readonly ICountryRepository _countryRepo;
        private readonly IRegionRepository _regionRepo;

        public CountryValidatorUp(ICountryRepository countryRepo, IRegionRepository regionRepository)
        {
            _countryRepo = countryRepo;
            _regionRepo = regionRepository;

            RuleFor(command => command.Idcountry)
                .Must(IsRightCountry)
                .WithMessage("Invalid value for Country field.\n")
                .NotNull()
                .WithMessage("Countryid is mandatory.\n");
            RuleFor(command => command).Must(IsCountryByRegion);
        }

        private bool IsCountryByRegion(UpdateOfferCommand obj) {

            var country = _regionRepo.GetCountryByRegion(obj.Idregion);
            bool canSetCountry = country != -1 && obj.Idregion != (int)Regions.AllCountry && obj.Idregion != (int)Regions.Abroad;

            if (canSetCountry)
                obj.Idcountry = country;
            return true;

        }

        private bool IsRightCountry(int _countryId)
        {
            return _countryRepo.IsRightCountry(_countryId);
        }
    }
}
