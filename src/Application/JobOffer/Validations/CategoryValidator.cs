using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class CategoryValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly IJobCategoryRepository _jobCatRepo;

        public CategoryValidator(IJobCategoryRepository jobCatRepo)
        {
            _jobCatRepo = jobCatRepo;
            RuleFor(command => command.IdjobCategory)
                .Must(IsRightCategory)
                .WithMessage("Invalid value for JobCategory field.\n")
                .NotNull()
                .WithMessage("JobCategoryId is mandatory.\n");
        }

        private bool IsRightCategory(int? _jobCatId)
        {
            return _jobCatRepo.IsRightCategory(_jobCatId);
        }
    }
}
