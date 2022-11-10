using Application.JobOffer.Commands;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.JobOffer.Validations
{
    public class DefaultCheckValuesValidator : AbstractValidator<CreateOfferCommand>
    {
        private IContractRepository _contractRepository;
        private ICityRepository _cityRepository;    

        public DefaultCheckValuesValidator(IContractRepository contractRepository,
            ICityRepository cityRepository)
        {
            _contractRepository = contractRepository;
            _cityRepository = cityRepository;

            RuleFor(command => command)
                .Must(HasDefaultValues)
                .WithMessage("Couldn't set default values.\n")
                .Must(HasCity).WithMessage("Have to have a city.");
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

        private bool HasCity(CreateOfferCommand obj)
        {
            if (string.IsNullOrEmpty(obj.City))
            {
                if(obj.Idcity != null)
                    obj.City = _cityRepository.GetName((int)obj.Idcity);
                
            }
            return !string.IsNullOrEmpty(obj.City);
        }
    }

    public class DefaultCheckValuesValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private IContractRepository _contractRepository;
        private ICityRepository _cityRepository;
        public DefaultCheckValuesValidatorUp(IContractRepository contractRepository, ICityRepository cityRepository)
        {
            _contractRepository = contractRepository;
            _cityRepository = cityRepository;

            RuleFor(command => command)
                .Must(HasDefaultValues)
                .WithMessage("Couldn't set default values.\n").Must(HasCity).WithMessage("Have to have a city.");
        }

        private bool HasCity(UpdateOfferCommand obj)
        {
            if (string.IsNullOrEmpty(obj.City))
            {
                if (obj.Idcity != null && obj.Idcity > 0)
                    obj.City = _cityRepository.GetName((int)obj.Idcity);
            }
            return !string.IsNullOrEmpty(obj.City) || obj.Idcity == 0;
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
