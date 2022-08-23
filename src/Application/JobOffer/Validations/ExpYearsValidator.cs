using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{

    public class ExpYearsValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly IJobExpYearsRepository _jobExpRepo;
        public ExpYearsValidator(IJobExpYearsRepository jobExpRepo)
        {
            _jobExpRepo = jobExpRepo;
            RuleFor(command => command.IdjobExpYears)
                .Must(IsRightExperienceYears)
                .WithMessage("Invalid value for JobExperienceYears field.\n")
                .NotNull()
                .WithMessage("JobExperienceYearsID is mandatory.\n");
        }

        private bool IsRightExperienceYears(int _expYearsId)
        {
            return _jobExpRepo.IsRightExperienceYears(_expYearsId);
        }
    }

    public class ExpYearsValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private readonly IJobExpYearsRepository _jobExpRepo;
        public ExpYearsValidatorUp(IJobExpYearsRepository jobExpRepo)
        {
            _jobExpRepo = jobExpRepo;
            RuleFor(command => command.IdjobExpYears)
                .Must(IsRightExperienceYears)
                .WithMessage("Invalid value for JobExperienceYears field.\n")
                .NotNull()
                .WithMessage("JobExperienceYearsID is mandatory.\n");
        }

        private bool IsRightExperienceYears(int _expYearsId)
        {
            return _jobExpRepo.IsRightExperienceYears(_expYearsId);
        }
    }

}

