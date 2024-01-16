using Application.AuxiliaryData.Queries;
using Application.Interfaces;
using Application.JobOffer.DTO;
using Application.JobOffer.Queries;
using Application.Utils;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
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
        private readonly IInternationalDiffusionCountryRepository _internationalDiffusionCountryRepository;
        private readonly IEnterpriseRepository _enterpriseRepository;
        private readonly IMediator _mediatr;
        private readonly IConfiguration _config;
        private readonly IJobVacancyLanguageRepository _jobVacancyLangRepo;
        private readonly IRegJobVacWorkPermitRepository _regJobVacWorkPermitRepo;
        private readonly ICityRepository _cityRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly IQuestService _questService;
        private readonly IApiUtils _utils;

        #endregion PRIVATE PROPERTIES

        public CreateOfferCommandHandler(IRegEnterpriseContractRepository regContractRepo,
            IRegJobVacMatchingRepository regJobVacRepo,
            IInternationalDiffusionCountryRepository internationalDiffusionCountryRepository,
            IMapper mapper,
            IJobOfferRepository offerRepo,
            IEnterpriseRepository enterpriseRepository,
            ILogger<CreateOfferCommandHandler> logger,
            IMediator mediatr, 
            IConfiguration config,
            IJobVacancyLanguageRepository jobVacancyLangRepo,
            IRegJobVacWorkPermitRepository regJobVacWorkPermitRepository,
            ICityRepository cityRepository,
            IAreaRepository areaRepository,
            IQuestService questService,
            IApiUtils utils)
        {
            _offerRepo = offerRepo;
            _mapper = mapper;
            _regContractRepo = regContractRepo;
            _regJobVacRepo = regJobVacRepo;
            _internationalDiffusionCountryRepository = internationalDiffusionCountryRepository;
            _enterpriseRepository = enterpriseRepository;
            _logger = logger;
            _mediatr = mediatr;            
            _config = config;
            _jobVacancyLangRepo = jobVacancyLangRepo;
            _regJobVacWorkPermitRepo = regJobVacWorkPermitRepository;
            _cityRepository = cityRepository;
            _areaRepository = areaRepository;
            _questService = questService;
            _utils = utils;
        }

        public async Task<OfferModificationResult> Handle(CreateOfferCommand offer, CancellationToken cancellationToken)
        {
            string error = string.Empty;
            int jobVacancyId = 0;
            var job = new JobVacancy();
            var integrationInfo = await _regJobVacRepo.GetAtsIntegrationInfo(offer.IntegrationData.ApplicationReference);
            // var company = _enterpriseRepository.Get(offer.Identerprise);
            //if (company.Idstatus != (int)EnterpriseStatus.Active)
            //{
            //    offer.Idstatus = (int)EnterpriseStatus.Pending;
            //}
            offer.Idstatus = (int)OfferStatus.Active;
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
                CityValidation(offer);
                var entity = _mapper.Map(offer, job);
                entity.IntegrationId = offer.IntegrationData.IDIntegration;
                jobVacancyId = _offerRepo.Add(entity);

                bool canSaveLanguages = jobVacancyId > 0
                     && offer.JobLanguages.Any()
                     && (offer.IntegrationData == null || string.IsNullOrEmpty(offer.IntegrationData.ApplicationReference));

                bool canSaveWorkPermit = jobVacancyId > 0
                    && offer.IdworkPermit.Any()
                    && offer.IntegrationData == null;

                if (canSaveLanguages)
                {
                    Savelanguages(offer, jobVacancyId);
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
                        if (offer.InternationalDiffusion ?? true)
                        {
                            await _internationalDiffusionCountryRepository.RemoveByOffer(jobVacancyId);
                            if(offer.InternationalDiffusionCountries?.Count > 0)
                            {
                                List<InternationalDiffusionCountry> listCountries = new List<InternationalDiffusionCountry>();
                                foreach(int internationalDiffusionCountry in offer.InternationalDiffusionCountries)
                                {
                                    listCountries.Add(new InternationalDiffusionCountry()
                                    {
                                        OfferId = jobVacancyId,
                                        CountryId = internationalDiffusionCountry
                                    });
                                }
                                await _internationalDiffusionCountryRepository.Add(listCountries);
                            }
                        }

                        await _regContractRepo.UpdateUnits(job.Idcontract, job.IdjobVacType);

                        if (!string.IsNullOrEmpty(offer.IntegrationData.ApplicationReference))
                        {
                            await IntegrationActions(offer, job);
                        }
                        var createdOffer = await _mediatr.Send(new GetResult.Query
                        {
                            ExternalId = offer.IntegrationData.ApplicationReference,
                            OfferId = jobVacancyId
                        });
                        _enterpriseRepository.UpdateATS(entity.Identerprise);                       

                        // QUESTIONNAIRE.
                        if (offer.QuestDTO != null)
                        {
                            int questId = await _questService.CreateQuest(offer.QuestDTO);
                            if (questId > 0)
                            {
                                job = _offerRepo.GetOfferById(jobVacancyId);
                                job.Idquest = questId;
                                await _offerRepo.UpdateOffer(job);
                            }
                            else
                            {
                                job = _offerRepo.GetOfferById(jobVacancyId);
                                job.Idstatus = 2;
                                await _offerRepo.UpdateOffer(job);
                                RegJobVacMatching jobReg = await _regJobVacRepo.GetAtsIntegrationInfoByJobId(jobVacancyId);
                                jobReg.ExternalId = $"{jobReg.ExternalId}_OLD";
                                await _regJobVacRepo.Update(jobReg);
                                error = "Failed to creating questionnaire";
                                _logger.LogError(error);
                                return OfferModificationResult.Failure(new List<string> { error });
                            }
                        }

                        // Google API Indexing URL Create/Update.
                        _utils.UpdateGoogleIndexingURL(_utils.GetOfferModel(offer));

                        return OfferModificationResult.Success(createdOffer);
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

        /// <summary>
        /// It puts city name
        /// </summary>
        /// <param name="offer"></param>
        private void CityValidation(CreateOfferCommand offer)
        {
            if (string.IsNullOrEmpty(offer.City.Trim()))
            {
                if (offer.Idcity != null && offer.Idcity > 0)
                    offer.City = _cityRepository.GetName((int)offer.Idcity);
                else offer.City = string.Empty;
            }

            if (!string.IsNullOrEmpty(offer.JobLocation) && !string.IsNullOrEmpty(offer.City))
            {
                offer.JobLocation = String.Empty;
            }
        }

        /// <summary>
        /// It saves languages if needed
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="jobVacancyId"></param>
        private void Savelanguages(CreateOfferCommand offer, int jobVacancyId)
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

        /// <summary>
        /// Actions regarding integrations
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="jobVacancyId"></param>
        /// <param name="job"></param>
        /// <returns></returns>
        private async Task IntegrationActions(CreateOfferCommand offer, JobVacancy job)
        {
            RegJobVacMatching obj = new RegJobVacMatching
            {
                ExternalId = offer.IntegrationData.ApplicationReference,
                Idintegration = offer.IntegrationData.IDIntegration,
                IdjobVacancy = job.IdjobVacancy,
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
    }
}
