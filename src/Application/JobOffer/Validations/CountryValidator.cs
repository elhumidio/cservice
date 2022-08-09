using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class CountryValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly ICountryRepository _countryRepo;

        public CountryValidator(ICountryRepository countryRepo)
        {
            _countryRepo = countryRepo;
            RuleFor(command => command.Idcountry)
                .Must(IsRightCountry)
                .WithMessage("Invalid value for Country field.\n")
                .NotNull()
                .WithMessage("Countryid is mandatory.\n");
        }

        private bool IsRightCountry(int _countryId)
        {
            return _countryRepo.IsRightCountry(_countryId);
        }
    }
}
