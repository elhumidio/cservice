using Application.JobOffer.Commands;
using Application.JobOffer.Queries;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.JobOffer.Validations
{
    public class DatesValidator : AbstractValidator<CreateOfferCommand>
    {
        private IMediator _mediator;
        private readonly IContractProductRepository _contractProdRepo;

        public DatesValidator(IMediator mediator, IContractProductRepository contractProdRepo)
        {
            _mediator = mediator;
            _contractProdRepo = contractProdRepo;

            RuleFor(command => command)
                .Must(HasRightFinishDate)
                .WithMessage("Couldn't establish right finish date.\n");
        }

        private bool HasRightFinishDate(CreateOfferCommand obj)
        {
            var productId = _contractProdRepo.GetIdProductByContract(obj.Idcontract);

            if (obj.IdjobVacancy != 0)
            {
                obj.FinishDate = _mediator.Send(new Get.Query
                {
                    OfferId = obj.IdjobVacancy,
                }).Result.FinishDate;
            }
            else
            {
                obj.FinishDate = _mediator.Send(new CalculateFinishDateOffer.Query
                {
                    ContractID = obj.Idcontract,
                    ProductId = productId,
                }).Result.Value;
            }
            obj.UpdatingDate = DateTime.Now;
            obj.PublicationDate = DateTime.Now;
            obj.UpdatingDate = DateTime.Now;
            obj.LastVisitorDate = null;
            obj.FilledDate = null;
            obj.ModificationDate = DateTime.Now;
            obj.LastVisitorDate = DateTime.Now;
            return obj.FinishDate >= DateTime.Today ? true : false;
        }
    }

    public class DatesValidatorUp : AbstractValidator<UpdateOfferCommand>
    {
        private IMediator _mediator;
        private readonly IContractProductRepository _contractProdRepo;
        private readonly IJobOfferRepository _jobOfferRepo;

        public DatesValidatorUp(IMediator mediator, IContractProductRepository contractProdRepo, IJobOfferRepository jobOfferRepo)
        {
            _mediator = mediator;
            _contractProdRepo = contractProdRepo;
            _jobOfferRepo = jobOfferRepo;

            RuleFor(command => command)
                .Must(HasRightFinishDate)
                .WithMessage("Couldn't establish right finish date.\n");
        }

        private bool HasRightFinishDate(UpdateOfferCommand obj)
        {
            var offer = _jobOfferRepo.GetOfferById(obj.IdjobVacancy);
            if (obj.Idcontract == 0)
                obj.Idcontract = offer.Idcontract;
            var productId = _contractProdRepo.GetIdProductByContract(obj.Idcontract);
            var dto = _mediator.Send(new Get.Query
            {
                OfferId = obj.IdjobVacancy,
            }).Result;

            if (offer.ChkFilled)
            {
                var finishDate = _mediator.Send(new CalculateFinishDateOffer.Query
                {
                    ContractID = obj.Idcontract,
                    ProductId = productId,
                }).Result.Value;
                obj.FinishDate = finishDate;
            }
            else
            {
                obj.FinishDate = dto.FinishDate;
            }
            obj.LastVisitorDate = dto.LastVisitorDate;
            obj.PublicationDate = dto.PublicationDate;

            obj.UpdatingDate = DateTime.Now;
            obj.LastVisitorDate = null;
            obj.FilledDate = null;
            obj.ModificationDate = DateTime.Now;
            obj.LastVisitorDate = DateTime.Now;
            return obj.FinishDate > DateTime.Today ? true : false;
        }
    }
}
