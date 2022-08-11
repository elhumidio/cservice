using Application.JobOffer.Commands;
using Domain.Enums;
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
            RuleFor(command => command)
                .Must(IsRightDegree)
                .WithMessage("Invalid value for Degree field.\n");


        }

        private bool IsRightDegree(CreateOfferCommand cmd)
        {
            if (cmd.Iddegree == null || cmd.Iddegree < 1)
                cmd.Iddegree = (int)degrees.BachelorsDegree;
            return _degreeRepo.IsRightDegree(cmd.Iddegree);
        }
    }
}
