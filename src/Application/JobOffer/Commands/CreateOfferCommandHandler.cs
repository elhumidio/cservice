using Application.Aimwel.Interfaces;
using Application.JobOffer.DTO;
using Application.JobOffer.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.JobOffer.Commands
{
    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, OfferModificationResult>
    {
        #region PRIVATE PROPERTIES

        private readonly ILogger<CreateOfferCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IJobOfferRepository _offerRepo;
        private readonly IRegJobVacMatchingRepository _regJobVacRepo;
        private readonly IRegEnterpriseContractRepository _regContractRepo;
        private readonly IEnterpriseRepository _enterpriseRepository;
        private readonly IMediator _mediatr;
        private readonly IAimwelCampaign _manageCampaign;
        private readonly IConfiguration _config;
        private readonly IJobVacancyLanguageRepository _jobVacancyLangRepo;
        private readonly IRegJobVacWorkPermitRepository _regJobVacWorkPermitRepo;

        #endregion PRIVATE PROPERTIES

        public CreateOfferCommandHandler(IRegEnterpriseContractRepository regContractRepo,
            IRegJobVacMatchingRepository regJobVacRepo,
            IMapper mapper,
            IJobOfferRepository offerRepo,
            IEnterpriseRepository enterpriseRepository,
            ILogger<CreateOfferCommandHandler> logger,
            IMediator mediatr, IAimwelCampaign manageCampaign,
            IConfiguration config,
            IJobVacancyLanguageRepository jobVacancyLangRepo, IRegJobVacWorkPermitRepository regJobVacWorkPermitRepository)
        {
            _offerRepo = offerRepo;
            _mapper = mapper;
            _regContractRepo = regContractRepo;
            _regJobVacRepo = regJobVacRepo;
            _enterpriseRepository = enterpriseRepository;
            _logger = logger;
            _mediatr = mediatr;
            _manageCampaign = manageCampaign;
            _config = config;
            _jobVacancyLangRepo = jobVacancyLangRepo;
            _regJobVacWorkPermitRepo = regJobVacWorkPermitRepository;
        }

        public async Task<OfferModificationResult> Handle(CreateOfferCommand offer, CancellationToken cancellationToken)
        {
            string error = string.Empty;
            int jobVacancyId = 0;
            var job = new JobVacancy();
            var integrationInfo = await _regJobVacRepo.GetAtsIntegrationInfo(offer.IntegrationData.ApplicationReference);

            if (integrationInfo != null)
                error = $"IntegrationId: {offer.IntegrationData.IDIntegration} - Reference: {offer.IntegrationData.ApplicationReference}";

            if (integrationInfo != null && integrationInfo.IdjobVacancy > 0) //caso update ATS
            {
                var existentOfferAts = _offerRepo.GetOfferById(integrationInfo.IdjobVacancy);
                error += " - Offer already exists";
                _logger.LogError(error);
                return OfferModificationResult.Failure(new List<string> { "Offer already exists" });
            }
            else if (offer.IdjobVacancy == 0)
            {
                var entity = _mapper.Map(offer, job);
                entity.IntegrationId = offer.IntegrationData.IDIntegration;
                jobVacancyId = _offerRepo.Add(entity);

                bool canSaveLanguages = jobVacancyId > 0
                     && offer.JobLanguages.Any()
                     && offer.IntegrationData == null;

                bool canSaveWorkPermit = jobVacancyId > 0
                    && offer.IdworkPermit.Any()
                    && offer.IntegrationData == null;                  

                if (canSaveLanguages)
                {
                    foreach (var lang in offer.JobLanguages)
                    {
                        JobVacancyLanguage language = new JobVacancyLanguage
                        {
                            IdjobVacancy = jobVacancyId,
                            IdlangLevel = lang.IdLangLevel,
                            Idlanguage = lang.IdLanguage
                        };
                        _jobVacancyLangRepo.Add(language);
                    }
                }
                
                if (canSaveWorkPermit)
                {
                    foreach (var permit in offer.IdworkPermit)
                    {
                        var ret = await _regJobVacWorkPermitRepo.Add(new RegJobVacWorkPermit() { IdjobVacancy = jobVacancyId, IdworkPermit = permit });
                    }
                }

                if (jobVacancyId == -1)
                {
                    error = "Failed to create offer";
                    _logger.LogError(error);
                    return OfferModificationResult.Failure(new List<string> { error });
                }
                else
                {
                    try
                    {
                        await _regContractRepo.UpdateUnits(job.Idcontract, job.IdjobVacType);

                        if (!string.IsNullOrEmpty(offer.IntegrationData.ApplicationReference))
                        {
                            RegJobVacMatching obj = new RegJobVacMatching
                            {
                                ExternalId = offer.IntegrationData.ApplicationReference,
                                Idintegration = offer.IntegrationData.IDIntegration,
                                IdjobVacancy = jobVacancyId,
                                AppEmail = offer.IntegrationData.ApplicationEmail,
                                Identerprise = offer.Identerprise,
                                Redirection = offer.IntegrationData.ApplicationUrl
                            };
                            var integration = _regJobVacRepo.GetAtsIntegrationInfo(obj.ExternalId).Result;
                            if (integration == null)
                            {
                                var info = $"IDJobVacancy: {job.IdjobVacancy} - IdIntegration: {offer.IntegrationData.IDIntegration} - Reference: {offer.IntegrationData.ApplicationReference} - Offer ATS Created";
                                _logger.LogInformation(info);
                                await _regJobVacRepo.Add(obj);
                            }
                            else
                            {
                                var info = $"IDJobVacancy: {job.IdjobVacancy} - IDEnterprise: {job.Identerprise} - IDContract: {job.Idcontract} - Offer Created";
                                _logger.LogInformation(info);
                            }
                        }
                        var createdOffer = _mediatr.Send(new GetResult.Query
                        {
                            ExternalId = offer.IntegrationData.ApplicationReference,
                            OfferId = jobVacancyId
                        });
                        _enterpriseRepository.UpdateATS(entity.Identerprise);
                        if (Convert.ToBoolean(_config["Aimwel:EnableAimwel"]))
                            await _manageCampaign.CreateCampaing(entity);
                        return OfferModificationResult.Success(createdOffer.Result);
                    }
                    catch (Exception ex)
                    {
                        error = $"message: Couldn't add Offer - Exception: {ex.Message} - {ex.InnerException}";
                        _logger.LogError(error);
                        return OfferModificationResult.Failure(new List<string> { error });
                    }
                }
            }
            else
            {
                error = $"Couldn't perform any operations, the offer {offer.IdjobVacancy} already exists.";
                _logger.LogError(error);
                return OfferModificationResult.Failure(new List<string> { error });
            }
        }
    }
}
