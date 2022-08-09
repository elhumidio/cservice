using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class DegreeValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly IDegreeRepository _degreeRepo;

        public DegreeValidator(IDegreeRepository degreeRepo)
        {
            _degreeRepo = degreeRepo;
            RuleFor(command => command.Idarea)
                .Must(IsRightDegree)
                .WithMessage("Invalid value for degree field.\n")
                .NotNull()
                .WithMessage("DegreeId is mandatory.\n");
        }

        private bool IsRightDegree(int _degreeId)
        {
            return _degreeRepo.IsRightDegree(_degreeId);
        }
    }
}
