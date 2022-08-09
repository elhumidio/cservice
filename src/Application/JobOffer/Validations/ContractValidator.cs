using Application.Contracts.Queries;
using Application.JobOffer.Commands;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.JobOffer.Validations
{
    public class ContractValidator : AbstractValidator<CreateOfferCommand>
    {
        private IMediator _mediator;

        private readonly IContractRepository _contractRepo;
        private readonly IContractProductRepository _contractProductRepo;

        public ContractValidator(IContractProductRepository contractProductRepo, IMediator mediator, IContractRepository contractRepo)
        {
            _mediator = mediator;
            _contractProductRepo = contractProductRepo;
            _contractRepo = contractRepo;

            RuleFor(command => command)
                .Must(IsValidContract)
                .WithMessage("Invalid value for IdContract field.\n")
                .NotNull()
                .WithMessage("ContractId is mandatory.\n");
            RuleFor(command => command)
                .Must(HasAvailableUnits)
                .WithMessage("Informed contract has not enough units for informed owner");
            RuleFor(command => command).Must(IsPack);
        }

        private bool IsValidContract(CreateOfferCommand obj)
        {
            return obj.IdenterpriseUserG > 0 && obj.Idcontract > 0 && _contractRepo.IsValidContract(obj.Idcontract);
        }

        private bool IsPack(CreateOfferCommand obj)
        {
            obj.ChkPack = _contractProductRepo.IsPack(obj.Idcontract);
            return true;
        }

        private bool HasAvailableUnits(CreateOfferCommand offer)
        {
            var units = _mediator.Send(new GetAvailableUnitsByOwner.Query
            {
                ContractId = offer.Idcontract,
                OwnerId = offer.IdenterpriseUserG.Value
            }).Result.Value;
            if (offer.IdjobVacType != -1)
            {
                var unitsAvailable = units.Where(u => (int)u.type == offer.IdjobVacType).ToList();
                int totalunits = unitsAvailable.Sum(u => u.Units);
                return totalunits > 0;
            }
            else
            {
                int totalunits = units.Sum(u => u.Units);
                var idtype = -1;
                foreach (var obj in units)
                {
                    idtype = obj.Units > 0 ? (int)obj.type : idtype;
                }
                offer.IdjobVacType = idtype;
                return totalunits > 0;

            } // assign first type available

        }
    }
}
