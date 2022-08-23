using Application.Core;
using AutoMapper;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.JobOffer.Commands
{
    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand, Result<string>>
    {
        #region PRIVATE PROPERTIES

        private readonly IMediator mediator;
        private readonly IMapper _mapper;
        private readonly IJobOfferRepository _offerRepo;
        private readonly IContractRepository _contractRepo;
        private readonly IProductRepository _productRepo;
        private readonly IValidator<CreateOfferCommand> _validator;
        private readonly IRegJobVacMatchingRepository _regJobVacRepo;
        private readonly IRegEnterpriseContractRepository _regContractRepo;
        private readonly IEnterpriseRepository _enterpriseRepository;
        private readonly IContractProductRepository _contractProductRepo;

        #endregion


        public UpdateOfferCommandHandler(IRegEnterpriseContractRepository regContractRepo,
            IRegJobVacMatchingRepository regJobVacRepo,
            IValidator<CreateOfferCommand> validator,
            IMapper mapper,
            IJobOfferRepository offerRepo,
            IContractRepository contractRepo,
            IProductRepository productRepo,
            IEnterpriseRepository enterpriseRepository, IContractProductRepository contractProductRepo)
        {
            _contractProductRepo = contractProductRepo;
            _contractRepo = contractRepo;
            _productRepo = productRepo;
            _offerRepo = offerRepo;
            _mapper = mapper;
            _regContractRepo = regContractRepo;
            _regJobVacRepo = regJobVacRepo;
            _enterpriseRepository = enterpriseRepository;
        }
        public async Task<Result<string>> Handle(UpdateOfferCommand offer, CancellationToken cancellationToken)
        {
            var existentOffer = _offerRepo.GetOfferById(offer.IdjobVacancy);
            bool IsActivate = existentOffer.ChkFilled;
            bool IsPack = _contractProductRepo.IsPack(existentOffer.Idcontract);
            bool CantUpdate = (IsActivate && !IsPack);
            if (CantUpdate)
            {
                return Result<string>.Failure("Failed to Update offer");
            }
            else
            {
                if (IsActivate)
                {
                    offer.FilledDate = null;
                    offer.FinishDate = existentOffer.FinishDate;
                    offer.LastVisitorDate = existentOffer.LastVisitorDate;
                    offer.ChkUpdateDate = existentOffer.ChkUpdateDate;
                    offer.ChkFilled = false;
                    offer.ChkDeleted = false;
                }
                var entity = _mapper.Map(offer, existentOffer);

                await _offerRepo.UpdateOffer(existentOffer);
                return Result<string>.Success("Updated");
            }
        }

    }
}
