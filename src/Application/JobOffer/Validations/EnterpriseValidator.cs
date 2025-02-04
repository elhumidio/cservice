using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;

namespace Application.JobOffer.Validations
{
    public class EnterpriseValidator : AbstractValidator<CreateOfferCommand>
    {
        private readonly IEnterpriseRepository _enterpriseRepo;

        public EnterpriseValidator(IEnterpriseRepository enterpriseRepo)
        {
            _enterpriseRepo = enterpriseRepo;
            RuleFor(command => command.Identerprise)
                .Must(IsRightCompany)
                .WithMessage("Invalid value for company field.\n")
                .NotNull()
                .WithMessage("CompanyId is mandatory.\n");
            RuleFor(command => command).Must(IsCompletedUserLastMod).WithMessage("Couldn't complete IdEnterpriseUserLastMode.\n");
        }

        private bool IsRightCompany(int companyId)
        {
            return companyId > 0 && _enterpriseRepo.IsRightCompany(companyId);
        }

        private bool IsCompletedUserLastMod(CreateOfferCommand obj)
        {
            obj.IdenterpriseUserLastMod = (int)obj.IdenterpriseUserG;
            return true;
        }
    }

    public class EnterpriseValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private readonly IEnterpriseRepository _enterpriseRepo;

        public EnterpriseValidatorUp(IEnterpriseRepository enterpriseRepo)
        {
            _enterpriseRepo = enterpriseRepo;
            RuleFor(command => command.Identerprise)
                .Must(IsRightCompany)
                .WithMessage("Invalid value for company field.\n")
                .NotNull()
                .WithMessage("CompanyId is mandatory.\n");
            RuleFor(command => command).Must(IsCompletedUserLastMod).WithMessage("Couldn't complete IdEnterpriseUserLastMode.\n");
        }

        private bool IsRightCompany(int companyId)
        {
            return companyId > 0 && _enterpriseRepo.IsRightCompany(companyId);
        }

        private bool IsCompletedUserLastMod(UpdateOfferCommand obj)
        {
            obj.IdenterpriseUserLastMod = (int)obj.IdenterpriseUserG;
            return true;
        }
    }
}
