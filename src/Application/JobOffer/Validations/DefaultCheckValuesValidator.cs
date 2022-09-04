using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using Domain.Enums;

namespace Application.JobOffer.Validations
{
    public class DefaultCheckValuesValidator : AbstractValidator<CreateOfferCommand>
    {
        IContractRepository _contractRepository;
        public DefaultCheckValuesValidator(IMediator mediator, IContractProductRepository contractProdRepo, IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
            RuleFor(command => command)
                .Must(HasDefaultValues)
                .WithMessage("Couldn't set default values.\n");
        }

        private bool HasDefaultValues(CreateOfferCommand obj)
        {
            //TODO get list of services

            var services = _contractRepository.GetServiceTypes(obj.Idcontract).ToList();
            obj.ChkBlindVac = false;
            obj.ChkFilled = false;
            obj.ChkDeleted = false;
            obj.ChkEnterpriseVisible = true;
            obj.ChkBlindSalary = false;
            obj.ChkDisability = false;
            obj.ChkUpdateDate = services.Where(a => a.ServiceType == (int)ServiceTypes.ManualJobRefresh).Any() ? true : false;
            return true;
        }
    }
    public class DefaultCheckValuesValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        public DefaultCheckValuesValidatorUp(IMediator mediator, IContractProductRepository contractProdRepo)
        {

            RuleFor(command => command)
                .Must(HasDefaultValues)
                .WithMessage("Couldn't set default values.\n");
        }

        private bool HasDefaultValues(UpdateOfferCommand obj)
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
