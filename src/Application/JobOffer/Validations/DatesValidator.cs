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
            //TODO get product from Contract
            var productId = _contractProdRepo.GetIdProductByContract(obj.Idcontract);
            var finishDate = _mediator.Send(new CalculateFinishDateOffer.Query
            {
                ContractID = obj.Idcontract,
                ProductId = productId,

            }).Result.Value;
            obj.FinishDate = finishDate;
            obj.UpdatingDate = DateTime.Now;
            obj.PublicationDate = DateTime.Now;
            obj.UpdatingDate = DateTime.Now;
            obj.LastVisitorDate = null;
            obj.FilledDate = null;
            obj.ModificationDate = DateTime.Now;
            obj.LastVisitorDate = DateTime.Now;

            return finishDate > DateTime.Today ? true : false;
        }
    }
}
