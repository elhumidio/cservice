using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class BrandValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly IBrandRepository _brandRepo;

        public BrandValidator(IBrandRepository brandRepo)
        {
            _brandRepo = brandRepo;
            RuleFor(command => command)
                .Must(IsRightBrand)
                .WithMessage("Invalid value for Brand field.\n")
                .NotNull()
                .WithMessage("BrandId is mandatory.\n");
        }

        private bool IsRightBrand(CreateOfferCommand obj)
        {
            return _brandRepo.IsRightBrand(obj.Idbrand, obj.Identerprise);
        }
    }
}
