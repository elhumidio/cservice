using Application.JobOffer.DTO;
using Application.JobOffer.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.JobOffer.Commands
{
    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand, OfferModificationResult>
    {
        #region PRIVATE PROPERTIES

        private readonly ILogger<UpdateOfferCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IJobOfferRepository _offerRepo;
        private readonly IProductRepository _productRepo;
        private readonly IRegJobVacMatchingRepository _regJobVacRepo;
        private readonly IRegEnterpriseContractRepository _regContractRepo;
        private readonly IContractProductRepository _contractProductRepo;
        private readonly IMediator _mediatr;

        #endregion PRIVATE PROPERTIES

        public UpdateOfferCommandHandler(IRegEnterpriseContractRepository regContractRepo,
            IRegJobVacMatchingRepository regJobVacRepo,
            IMapper mapper,
            IJobOfferRepository offerRepo,
            IEnterpriseRepository enterpriseRepository,
            IContractProductRepository contractProductRepo,
            ILogger<UpdateOfferCommandHandler> logger,
            IMediator mediatr)
        {
            _contractProductRepo = contractProductRepo;
            _offerRepo = offerRepo;
            _mapper = mapper;
            _regContractRepo = regContractRepo;
            _regJobVacRepo = regJobVacRepo;
            _mediatr = mediatr;

            _logger = logger;
        }

        public async Task<OfferModificationResult> Handle(UpdateOfferCommand offer, CancellationToken cancellationToken)
        {
            string error = string.Empty;
            var integrationInfo = await _regJobVacRepo.GetAtsIntegrationInfo(offer.IntegrationData.ApplicationReference);

            if (integrationInfo != null)
                error = $"IntegrationId: {offer.IntegrationData.IDIntegration} - Reference: {offer.IntegrationData.ApplicationReference}";

            var existentOffer = _offerRepo.GetOfferById(offer.IdjobVacancy);
            bool IsActivate = existentOffer.ChkFilled;
            bool IsPack = _contractProductRepo.IsPack(existentOffer.Idcontract);

            if (IsActivate)
            {
                offer.FilledDate = null;
                offer.ChkUpdateDate = existentOffer.ChkUpdateDate;
                offer.ChkFilled = false;
                offer.ChkDeleted = false;
                offer.ChkPack = IsPack;

                await _regContractRepo.UpdateUnits(offer.Idcontract, offer.IdjobVacType);
            }
            var entity = _mapper.Map(offer, existentOffer);

            var ret = await _offerRepo.UpdateOffer(existentOffer);
            var createdOffer = _mediatr.Send(new GetResult.Query
            {
                ExternalId = offer.IntegrationData.ApplicationReference,
                OfferId = offer.IdjobVacancy
            });
            
            await _regContractRepo.UpdateUnits(offer.Idcontract, offer.IdjobVacType);
            if (ret < 0)
                return OfferModificationResult.Failure(new List<string> { "no update" });
            else
                return OfferModificationResult.Success(createdOffer.Result);
        }
    }
}
