using Application.Contracts.Queries;
using Application.JobOffer.Commands;
using Domain.Enums;
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
        private readonly IUserRepository _userRepo;
        private readonly IUnitsRepository _unitsRepo;
        private readonly IRegJobVacMatchingRepository _jobmatchAtsRepo;
        private readonly IJobOfferRepository _jobRepo;

        public ContractValidator(IContractProductRepository contractProductRepo,
            IMediator mediator,
            IContractRepository contractRepo,
            IUserRepository userRepo,
            IUnitsRepository unitsRepo,
            IRegJobVacMatchingRepository jobMatchAtsRepo, IJobOfferRepository jobRepo)
        {
            _mediator = mediator;
            _contractProductRepo = contractProductRepo;
            _contractRepo = contractRepo;
            _userRepo = userRepo;
            _unitsRepo = unitsRepo;
            _jobmatchAtsRepo = jobMatchAtsRepo;
            _jobRepo = jobRepo;

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
            bool IsValidEdit = false;

            var actualOfferAts =  _jobmatchAtsRepo.GetAtsIntegrationInfo(offer.IntegrationData.ApplicationReference).Result;
            if (actualOfferAts != null)
            {
                var offerDb = _jobRepo.GetOfferById(actualOfferAts.IdjobVacancy);
                IsValidEdit = offer.IdjobVacancy > 0 || offerDb != null;
                if (IsValidEdit && !offerDb.ChkFilled)
                {
                    return true;
                }
            }

            int totalunits = 0;
            bool ans = false;
            var units = _mediator.Send(new GetAvailableUnits.Query
            {
                ContractId = offer.Idcontract,
            }).Result.Value.Where(u => u.OwnerId == offer.IdenterpriseUserG);
            var unitsAvailable = units.Sum(u => u.Units);

            if (unitsAvailable > 0)
            {
                //el user tiene unidades del mismo tipo
                var unitsSameTypemanager = units.Where(u => u.OwnerId == offer.IdenterpriseUserG && offer.IdjobVacType == (int)u.type);
                var unitsAssignedOtherKind = units.Where(u => u.OwnerId == offer.IdenterpriseUserG && offer.IdjobVacType != (int)u.type);
                var bestUnits = unitsSameTypemanager.Sum(unit => unit.Units);

                switch (bestUnits)
                {
                    case > 0:
                        {
                            var assignment = unitsSameTypemanager.Where(u => u.Units > 0).OrderByDescending(y => y.Units).FirstOrDefault();

                            if (assignment != null)
                            {
                                offer.IdjobVacType = (int)assignment.type;
                                return true;
                            }
                            else return false;
                        }

                    case 0:
                        {
                            //verificar si alguien tiene del mismo tipo

                            var unitsAssignedAnyKind = units.Where(u => u.Units > 0 && u.type == (VacancyType)offer.IdjobVacType)
                                .OrderByDescending(d => d.Units);
                            if (unitsAssignedAnyKind != null && unitsAssignedAnyKind.Any())
                            {
                                var unitsToUse = unitsAssignedAnyKind.First();
                                offer.IdjobVacType = (int)unitsToUse.type;
                                _unitsRepo.TakeUnitFromManager(offer.Idcontract, unitsToUse.type, unitsToUse.OwnerId);
                                offer.IdenterpriseUserG = unitsToUse.OwnerId;
                                offer.IdenterpriseUserLastMod = unitsToUse.OwnerId;
                                _unitsRepo.AssignUnitToManager(offer.Idcontract, unitsToUse.type, (int)offer.IdenterpriseUserG);
                                return true;
                            }
                            else
                            {
                                if (unitsAssignedOtherKind.Sum(unit => unit.Units) > 0)
                                {
                                    var unitsToUse = unitsAssignedOtherKind.First();
                                    offer.IdenterpriseUserG = unitsToUse.OwnerId;
                                    offer.IdenterpriseUserLastMod = unitsToUse.OwnerId;
                                    offer.IdjobVacType = (int)unitsAssignedOtherKind.First().type;
                                    _unitsRepo.TakeUnitFromManager(offer.Idcontract, unitsToUse.type, unitsToUse.OwnerId);
                                    _unitsRepo.AssignUnitToManager(offer.Idcontract, unitsToUse.type, (int)offer.IdenterpriseUserG);
                                    return true;
                                }
                            }

                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
                return false;
            }
            else return false;
        }
    }
}
