using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class JobTypeValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly IJobTypeRepository _jobTypeRepo;

        public JobTypeValidator(IJobTypeRepository jobTypeRepo)
        {
            _jobTypeRepo = jobTypeRepo;
            RuleFor(command => command.IdjobVacType)
                .Must(IsRightJobVacType)
                .WithMessage("Invalid value for VacType field.\n")
                .NotNull()
                .WithMessage("JobVacTypeId is mandatory.\n");
        }

        private bool IsRightJobVacType(int _jobTypeId)
        {
            return _jobTypeRepo.IsRightJobVacType(_jobTypeId);
        }
    }


    public class JobTypeValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private readonly IJobTypeRepository _jobTypeRepo;

        public JobTypeValidatorUp(IJobTypeRepository jobTypeRepo)
        {
            _jobTypeRepo = jobTypeRepo;
            RuleFor(command => command.IdjobVacType)
                .Must(IsRightJobVacType)
                .WithMessage("Invalid value for VacType field.\n")
                .NotNull()
                .WithMessage("JobVacTypeId is mandatory.\n");
        }

        private bool IsRightJobVacType(int _jobTypeId)
        {
            return _jobTypeRepo.IsRightJobVacType(_jobTypeId);
        }
    }
}
