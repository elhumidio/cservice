using Application.JobOffer.Commands;
using FluentValidation;
using Persistence.Enums;
namespace Application.JobOffer.Validations
{
    public class RegistrationTypeValidator : AbstractValidator<CreateOfferCommand>
    {
        public RegistrationTypeValidator()
        {
            RuleFor(command => command).Must(IsAcceptedType);
        }

        private bool IsAcceptedType(CreateOfferCommand obj)
        {
            if (!Enum.IsDefined(typeof(RegistrationType), (RegistrationType)obj.IdjobRegType))
            {
                obj.IdjobRegType = (int)RegistrationType.Classic;
            }
            return true;

        }
    }
}
