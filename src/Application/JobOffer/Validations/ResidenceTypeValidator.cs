using Application.JobOffer.Commands;
using Domain.Enums;
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
            if (!Enum.IsDefined(typeof(ResidenceType), obj.IdresidenceType))
            {
                obj.IdresidenceType = (int)ResidenceType.Indiferent;
                return true;
            }

            else
                return _residenceTypeRepo.IsRightResidenceType(obj.IdresidenceType);
        }
    }

    public class ResidenceTypeValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private readonly IResidenceTypeRepository _residenceTypeRepo;
        public ResidenceTypeValidatorUp(IResidenceTypeRepository residenceTypeRepo)
        {
            _residenceTypeRepo = residenceTypeRepo;

            RuleFor(command => command)
                .NotEmpty()
                .WithMessage("Invalid value for residence type field.\n\n")
                .Must(IsRightResidenceType)
                .WithMessage("ResidenceType is mandatory field");
        }

        private bool IsRightResidenceType(UpdateOfferCommand obj)
        {
            if (!Enum.IsDefined(typeof(ResidenceType), obj.IdresidenceType))
            {
                obj.IdresidenceType = (int)ResidenceType.Indiferent;
                return true;
            }

            else
                return _residenceTypeRepo.IsRightResidenceType(obj.IdresidenceType);
        }
    }
}
