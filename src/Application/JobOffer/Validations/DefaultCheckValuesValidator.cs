using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.JobOffer.Validations
{
    public class DefaultCheckValuesValidator : AbstractValidator<CreateOfferCommand>
    {
        public DefaultCheckValuesValidator(IMediator mediator, IContractProductRepository contractProdRepo)
        {

            RuleFor(command => command)
                .Must(HasDefaultValues)
                .WithMessage("Couldn't set default values.\n");
        }

        private bool HasDefaultValues(CreateOfferCommand obj)
        {
            obj.ChkBlindVac = false;
            obj.ChkFilled = false;
            obj.ChkDeleted = false;
            obj.ChkEnterpriseVisible = true;
            obj.ChkBlindSalary = false;
            obj.ChkDisability = false;
            obj.ChkUpdateDate = true;
            return true;
        }
    }
}
