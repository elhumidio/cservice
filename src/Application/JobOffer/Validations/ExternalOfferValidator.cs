using Application.JobOffer.Commands;
using Application.Utils;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class ExternalOfferValidator : AbstractValidator<CreateOfferCommand>
    {
        public ExternalOfferValidator()
        {
            RuleFor(command => command.IntegrationData.ApplicationEmail)
                .Must(IsValidApplicationEmail)
                .WithMessage("Field ApplicationEmail is wrong formatted.\n");
        }

        public bool IsValidApplicationEmail(string _email)
        {
            bool ans = false;
            if (string.IsNullOrEmpty(_email))
                ans = true;
            else
            {
                ans = ApiUtils.IsValidEmail(_email);
            }

            return ans;

        }

    }

    public class ExternalOfferValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        public ExternalOfferValidatorUp()
        {
            RuleFor(command => command.IntegrationData.ApplicationEmail)
                .Must(IsValidApplicationEmail)
                .WithMessage("Field ApplicationEmail is wrong formatted.\n");
        }

        public bool IsValidApplicationEmail(string _email)
        {
            bool ans = false;
            if (string.IsNullOrEmpty(_email))
                ans = true;
            else
            {
                ans = ApiUtils.IsValidEmail(_email);
            }

            return ans;

        }

    }
}
