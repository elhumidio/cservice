using Application.JobOffer.Commands;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class VacancyNumberValidator : AbstractValidator<CreateOfferCommand>
    {
        public VacancyNumberValidator()
        {
            RuleFor(command => command.VacancyNumber)
                .NotEmpty()
                .WithMessage("Wrong format for number of vacancies field.\n")
                .InclusiveBetween(1, 100)
                .WithMessage("Value should be between 1-100");
            RuleFor(command => command).Must(HasVacancies).WithMessage("Vacancy number should be greater than zero.\n");
        }
        private bool HasVacancies(CreateOfferCommand obj)
        {
            if (obj.VacancyNumber <= 0)
            {
                obj.VacancyNumber = 1;
            }
            return obj.VacancyNumber > 0;
        }
    }
}
