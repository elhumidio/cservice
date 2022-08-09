using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class ResidenceTypeValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly IResidenceTypeRepository _residenceTypeRepo;
        public ResidenceTypeValidator(IResidenceTypeRepository residenceTypeRepo)
        {
            _residenceTypeRepo = residenceTypeRepo;

            RuleFor(command => command)
                .NotEmpty()
                .WithMessage("Invalid value for residence type field.\n\n")
                .Must(IsRightResidenceType)
                .WithMessage("ResidenceType is mandatory field");
        }

        private bool IsRightResidenceType(CreateOfferCommand obj)
        {
            if (!Enum.IsDefined(typeof(Persistence.Enums.ResidenceType), obj.IdresidenceType))
            {
                obj.IdresidenceType = (int)Persistence.Enums.ResidenceType.Indiferent;
                return true;
            }

            else
                return _residenceTypeRepo.IsRightResidenceType(obj.IdresidenceType);
        }
    }
}
