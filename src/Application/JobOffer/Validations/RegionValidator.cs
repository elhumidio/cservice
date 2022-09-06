using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class RegionValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly IRegionRepository _regionRepo;

        public RegionValidator(IRegionRepository regionRepo)
        {
            _regionRepo = regionRepo;
            RuleFor(command => command.Idregion)
                .Must(IsRightRegion)
                .WithMessage("Invalid value for region field.\n")
                .NotNull()
                .WithMessage("Regionid is mandatory.\n");
        }

        private bool IsRightRegion(int _regionId)
        {
            return _regionRepo.IsRightRegion(_regionId);
        }
    }

    public class RegionValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private readonly IRegionRepository _regionRepo;

        public RegionValidatorUp(IRegionRepository regionRepo)
        {
            _regionRepo = regionRepo;
            RuleFor(command => command.Idregion)
                .Must(IsRightRegion)
                .WithMessage("Invalid value for region field.\n")
                .NotNull()
                .WithMessage("Regionid is mandatory.\n");
        }

        private bool IsRightRegion(int _regionId)
        {
            return _regionRepo.IsRightRegion(_regionId);
        }
    }
}
