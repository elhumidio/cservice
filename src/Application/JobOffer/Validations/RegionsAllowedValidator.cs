using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class RegionsAllowedValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly IContractPublicationRegionRepository _regionsAllowedRepo;

        public RegionsAllowedValidator(IContractPublicationRegionRepository regionsAllowedRepo)
        {
            _regionsAllowedRepo = regionsAllowedRepo;
            RuleFor(command => command)
                .Must(IsRegionAllowedByContract)
                .WithMessage("Region not allowed for current contract");
        }

        private bool IsRegionAllowedByContract(CreateOfferCommand obj)
        {
            var regions = _regionsAllowedRepo.AllowedRegionsByContract(obj.Idcontract);
            return (regions == null) || !regions.Any() || (regions.Any() && regions.Contains(obj.Idregion));
        }
    }

    public class RegionsAllowedValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private readonly IContractPublicationRegionRepository _regionsAllowedRepo;

        public RegionsAllowedValidatorUp(IContractPublicationRegionRepository regionsAllowedRepo)
        {
            _regionsAllowedRepo = regionsAllowedRepo;
            RuleFor(command => command)
                .Must(IsRegionAllowedByContract)
                .WithMessage("Region not allowed for current contract");
        }

        private bool IsRegionAllowedByContract(UpdateOfferCommand obj)
        {
            var regions = _regionsAllowedRepo.AllowedRegionsByContract(obj.Idcontract);
            return (regions == null) || !regions.Any() || (regions.Any() && regions.Contains(obj.Idregion));
        }
    }
}
