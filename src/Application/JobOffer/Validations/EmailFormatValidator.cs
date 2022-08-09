using Application.JobOffer.Commands;
using Application.Utils;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class EmailFormatValidator : AbstractValidator<CreateOfferCommand>
    {

        public EmailFormatValidator()
        {
            RuleFor(command => command.IntegrationData.ApplicationEmail).Must(IsRightFormat).WithMessage("ApplicationEmail is wrongly formatted.\n");
        }
        private static bool IsRightFormat(string _email)
        {
            if (string.IsNullOrEmpty(_email))
                return true;
            else
                return ApiUtils.IsValidEmail(_email);
        }
    }
}
