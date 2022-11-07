using Application.JobOffer.Commands;
using Domain.Enums;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.JobOffer.Validations
{
    public class DefaultCheckValuesValidator : AbstractValidator<CreateOfferCommand>
    {
        private IContractRepository _contractRepository;

        public DefaultCheckValuesValidator(IMediator mediator, IContractProductRepository contractProdRepo, IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
            RuleFor(command => command)
                .Must(HasDefaultValues)
                .WithMessage("Couldn't set default values.\n");
        }

        private bool HasDefaultValues(CreateOfferCommand obj)
        {
            var services = _contractRepository.GetServiceTypes(obj.Idcontract).ToList();
            obj.ChkBlindVac = obj.ChkBlindVac == null  ?  false : obj.ChkBlindVac;
            obj.ChkFilled = false;
            obj.ChkDeleted = false;
            obj.ChkEnterpriseVisible = true;
            obj.ChkBlindSalary = obj.ChkBlindSalary == null ?  false : obj.ChkBlindSalary;
            obj.ChkDisability = false;
            obj.ChkUpdateDate = services.Where(a => a.ServiceType == (int)ServiceTypes.ManualJobRefresh).Any() ? true : false;
            return true;
        }
    }

    public class DefaultCheckValuesValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private IContractRepository _contractRepository;
        public DefaultCheckValuesValidatorUp(IMediator mediator, IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
            RuleFor(command => command)
                .Must(HasDefaultValues)
                .WithMessage("Couldn't set default values.\n");
        }

        private bool HasDefaultValues(UpdateOfferCommand obj)
        {
            var services = _contractRepository.GetServiceTypes(obj.Idcontract).ToList();
            obj.ChkBlindVac = obj.ChkBlindVac == null ? true : obj.ChkBlindVac;
            obj.ChkFilled = obj.ChkFilled == null ? false : obj.ChkFilled;
            obj.ChkDeleted = obj.ChkFilled == null  ? false : obj.ChkDeleted;
            obj.ChkEnterpriseVisible = obj.ChkEnterpriseVisible == null ? true : obj.ChkEnterpriseVisible;
            obj.ChkBlindSalary = obj.ChkBlindSalary == null ? false : obj.ChkBlindSalary;
            obj.ChkDisability = obj.ChkDisability== null ?  false : obj.ChkDisability;
            obj.ChkUpdateDate = services.Where(a => a.ServiceType == (int)ServiceTypes.ManualJobRefresh).Any() ? true : false;
            return true;
        }
    }
}
