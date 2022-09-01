using Application.Core;
using AutoMapper;
using Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.JobOffer.Commands
{
    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand, Result<string>>
    {
        #region PRIVATE PROPERTIES

        private readonly ILogger<UpdateOfferCommandHandler> _logger;
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
            IEnterpriseRepository enterpriseRepository,
            IContractProductRepository contractProductRepo,
            ILogger<UpdateOfferCommandHandler> logger)
        {
            _contractProductRepo = contractProductRepo;
            _contractRepo = contractRepo;
            _productRepo = productRepo;
            _offerRepo = offerRepo;
            _mapper = mapper;
            _regContractRepo = regContractRepo;
            _regJobVacRepo = regJobVacRepo;
            _enterpriseRepository = enterpriseRepository;
            _logger = logger;
        }
        public async Task<Result<string>> Handle(UpdateOfferCommand offer, CancellationToken cancellationToken)
        {
            var existentOffer = _offerRepo.GetOfferById(offer.IdjobVacancy);
            bool IsActivate = existentOffer.ChkFilled;
            bool IsPack = _contractProductRepo.IsPack(existentOffer.Idcontract);
            bool CantUpdate = (IsActivate && !IsPack);
            if (CantUpdate)
            {
                var error = $"IntegrationId: {offer.IntegrationData.IDIntegration} - Reference: {offer.IntegrationData.ApplicationReference} - Failed to Update offer";
                _logger.LogError(error);
                return Result<string>.Failure("Failed to Update offer");
            }
            else
            {
                if (IsActivate)
                {
                    offer.FilledDate = null;                 
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
