using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class ContractTypeValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly IJobContractTypeRepository _contractTypeRepo;

        public ContractTypeValidator(IJobContractTypeRepository contractTypeRepo)
        {
            _contractTypeRepo = contractTypeRepo;
            RuleFor(command => command.IdjobContractType)
                .Must(IsRightContractType)
                .WithMessage("Invalid value for job industry field.")
                .NotNull()
                .WithMessage("contractTypeId is mandatory field. \n");
        }

        private bool IsRightContractType(int _contractType)
        {
            return _contractTypeRepo.IsRightContractType(_contractType);
        }
    }
    public class ContractTypeValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private readonly IJobContractTypeRepository _contractTypeRepo;

        public ContractTypeValidatorUp(IJobContractTypeRepository contractTypeRepo)
        {
            _contractTypeRepo = contractTypeRepo;
            RuleFor(command => command.IdjobContractType)
                .Must(IsRightContractType)
                .WithMessage("Invalid value for job industry field.")
                .NotNull()
                .WithMessage("contractTypeId is mandatory field. \n");
        }

        private bool IsRightContractType(int _contractType)
        {
            return _contractTypeRepo.IsRightContractType(_contractType);
        }
    }
}
