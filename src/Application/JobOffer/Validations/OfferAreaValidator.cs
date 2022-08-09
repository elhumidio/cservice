using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class OfferAreaValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly IAreaRepository _areaRepo;
        public OfferAreaValidator(IAreaRepository areaRepo)
        {
            _areaRepo = areaRepo;

            RuleFor(command => command.Idarea)
                .Must(IsRightArea)
                .WithMessage("Invalid value for job industry field.")
                .NotNull()
                .WithMessage("AreaId is mandatory field. \n");
        }

        private bool IsRightArea(int _areaId)
        {
            return _areaRepo.IsRightArea(_areaId);
        }

    }
}
