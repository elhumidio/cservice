using Application.EnterpriseContract.Queries;
using Application.JobOffer.DTO;
using Application.JobOffer.Queries;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.JobOffer.Commands
{
    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand, OfferModificationResult>
    {
        #region PRIVATE PROPERTIES

        private readonly IMapper _mapper;
        private readonly IJobOfferRepository _offerRepo;        
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
            _regContractRepo = regContractRepo;
            _regJobVacRepo = regJobVacRepo;
            _mediatr = mediatr;
            _mapper = mapper;
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
            offer.ChkPack = IsPack;

            if (IsActivate)
            {
                var result = await _mediatr.Send(new GetContract.Query
                {
                    CompanyId = offer.Identerprise,
                    type = (VacancyType)offer.IdjobVacType,
                    RegionId = offer.Idregion
                });
                bool canActivate = result.Value != null && result.Value.Idcontract > 0;
                if (canActivate)
                {
                    offer.FilledDate = null;
                    offer.ChkUpdateDate = existentOffer.ChkUpdateDate;
                    offer.ChkFilled = false;
                    offer.ChkDeleted = false;
                    offer.IdjobVacType = (int)result.Value.IdJobVacType;
                    offer.PublicationDate = DateTime.Now;
                    offer.UpdatingDate = DateTime.Now;  
                    offer.Idstatus = (int)OfferStatus.Active;
                    await _regContractRepo.UpdateUnits(result.Value.Idcontract, (int)result.Value.IdJobVacType);
                }
            }
            var entity = _mapper.Map(offer, existentOffer);

            var ret = await _offerRepo.UpdateOffer(existentOffer);
            var updatedOffer = _mediatr.Send(new GetResult.Query
            {
                ExternalId = offer.IntegrationData.ApplicationReference,
                OfferId = offer.IdjobVacancy
            }).Result;

            var integration = _regJobVacRepo.GetAtsIntegrationInfoByJobId(existentOffer.IdjobVacancy).Result;

            if (integration != null)
            {
                updatedOffer.IntegrationData.ApplicationEmail = integration.AppEmail;
                updatedOffer.IntegrationData.ApplicationReference = integration.ExternalId;
                updatedOffer.IntegrationData.ApplicationUrl = integration.Redirection;
                updatedOffer.IntegrationData.IDIntegration = integration.Idintegration;
            }

            if (ret < 0)
                return OfferModificationResult.Failure(new List<string> { "no update" });
            else
                return OfferModificationResult.Success(updatedOffer);
        }
    }
}
