using Application.JobOffer.Commands;
using Application.Utils;
using Domain.Enums;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class SalaryTypeValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly ISalaryTypeRepository _salaryTypeRepo;

        public SalaryTypeValidator(ISalaryTypeRepository salaryTypeRepo)
        {
            _salaryTypeRepo = salaryTypeRepo;
            //RuleFor(command => command).Must(IsRightMaxSalaryValue).WithMessage("Salary max are incorrect values");
            //RuleFor(command => command).Must(IsRightMinSalaryValue).WithMessage("Salary min are incorrect values");
            RuleFor(command => command)
                .Must(IsRightSalaryType)
                .WithMessage("Invalid value for salaryType field.\n")
                .NotNull()
                .WithMessage("IdsalaryType is mandatory.\n");
        }

        private bool IsRightSalaryType(CreateOfferCommand obj)
        {
            bool IsNotdefinedSalaries = obj.SalaryMax == ((decimal)SalaryType.NotSpecified) && obj.SalaryMin == ((decimal)SalaryType.NotSpecified);

            if (IsNotdefinedSalaries && obj.IdsalaryType != (int)SalaryType.UnpaidWork)
            {
                obj.IdsalaryType = (int)SalaryType.NotSpecified;
                return true;
            }
            else
                return _salaryTypeRepo.IsRightSalaryType(obj.IdsalaryType);
        }

        //THESE FUNCTIONS ARE NOT NECESSSARY ANYMORE BECAUSE THE FIELD IS OPEN TO FREE VALUES
        //private bool IsRightMaxSalaryValue(CreateOfferCommand obj)
        //{
        //    bool IsNotDefinedSalMax = string.IsNullOrEmpty(obj.SalaryMax) || Convert.ToInt32(obj.SalaryMax) == (int)SalaryType.NotSpecified;
        //    if (IsNotDefinedSalMax)
        //    {
        //        obj.SalaryMax = SalaryType.NotSpecified.NumberString();
        //        return true;
        //    }

        //    return _salaryTypeRepo.IsRightSalaryValue(obj.SalaryMax, obj.IdsalaryType) || obj.SalaryMax.Equals(string.Empty);
        //}

        //private bool IsRightMinSalaryValue(CreateOfferCommand obj)
        //{
        //    bool IsNotDefinedSalMin = string.IsNullOrEmpty(obj.SalaryMin) || Convert.ToInt32(obj.SalaryMin) == (int)SalaryType.NotSpecified;
        //    if (IsNotDefinedSalMin)
        //    {
        //        obj.SalaryMin = SalaryType.NotSpecified.NumberString();
        //        return true;
        //    }

        //    return (_salaryTypeRepo.IsRightSalaryValue(obj.SalaryMin, obj.IdsalaryType) || obj.SalaryMin.Equals(string.Empty));
        //}
    }

    public class SalaryTypeValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private readonly ISalaryTypeRepository _salaryTypeRepo;

        public SalaryTypeValidatorUp(ISalaryTypeRepository salaryTypeRepo)
        {
            _salaryTypeRepo = salaryTypeRepo;
            //RuleFor(command => command).Must(IsRightMaxSalaryValue).WithMessage("Salary max are incorrect values");
            //RuleFor(command => command).Must(IsRightMinSalaryValue).WithMessage("Salary min are incorrect values");
            RuleFor(command => command)
                .Must(IsRightSalaryType)
                .WithMessage("Invalid value for salaryType field.\n")
                .NotNull()
                .WithMessage("IdsalaryType is mandatory.\n");
        }

        private bool IsRightSalaryType(UpdateOfferCommand obj)
        {
            bool IsNotdefinedSalaries = obj.SalaryMax == (decimal)SalaryType.NotSpecified && obj.SalaryMin == (decimal)SalaryType.NotSpecified;

            if (IsNotdefinedSalaries)
            {
                obj.IdsalaryType = (int)SalaryType.NotSpecified;
                return true;
            }
            else
                return _salaryTypeRepo.IsRightSalaryType(obj.IdsalaryType);
        }

        //private bool IsRightMaxSalaryValue(UpdateOfferCommand obj)
        //{
        //    bool IsNotDefinedSalMax = string.IsNullOrEmpty(obj.SalaryMax) || Convert.ToInt32(obj.SalaryMax) == (int)SalaryType.NotSpecified;
        //    if (IsNotDefinedSalMax)
        //    {
        //        obj.SalaryMax = SalaryType.NotSpecified.NumberString();
        //        return true;
        //    }

        //    return _salaryTypeRepo.IsRightSalaryValue(obj.SalaryMax, obj.IdsalaryType) || obj.SalaryMax.Equals(string.Empty);
        //}

        //private bool IsRightMinSalaryValue(UpdateOfferCommand obj)
        //{
        //    bool IsNotDefinedSalMin = string.IsNullOrEmpty(obj.SalaryMin) || Convert.ToInt32(obj.SalaryMin) == (int)SalaryType.NotSpecified;
        //    if (IsNotDefinedSalMin)
        //    {
        //        obj.SalaryMin = SalaryType.NotSpecified.NumberString();
        //        return true;
        //    }

        //    return (_salaryTypeRepo.IsRightSalaryValue(obj.SalaryMin, obj.IdsalaryType) || obj.SalaryMin.Equals(string.Empty));
        //}
    }
}
