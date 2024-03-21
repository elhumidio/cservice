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
using System.Text.RegularExpressions;

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
        private readonly IJobTitleDenominationsRepository _denominationsRepository;
        private readonly IAIService _aiService;
        

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
            IApiUtils utils,
            IJobTitleDenominationsRepository denominationsRepository,
            IAIService aiService)
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
            _denominationsRepository = denominationsRepository;
            _aiService = aiService;
        }

        public async Task<OfferModificationResult> Handle(CreateOfferCommand offer, CancellationToken cancellationToken)
        {
            string error = string.Empty;
            int jobVacancyId = 0;
            var job = new JobVacancy();
            var integrationInfo = await _regJobVacRepo.GetAtsIntegrationInfo(offer.IntegrationData.ApplicationReference);
   
            offer.Idstatus = (int)OfferStatus.Active;
            if (integrationInfo != null)
                error = $"IntegrationId: {offer.IntegrationData.IDIntegration} - Reference: {offer.IntegrationData.ApplicationReference}";

            //Fail conditions
            if (integrationInfo != null && integrationInfo.IdjobVacancy > 0) //caso update ATS
            {
                var existentOfferAts = _offerRepo.GetOfferById(integrationInfo.IdjobVacancy);
                error += " - Offer already exists";
                _logger.LogError(error);
                return OfferModificationResult.Failure(new List<string> { "Offer already exists" });
            }
            if (offer.IdjobVacancy != 0)
            {
                error = $"Couldn't perform any operations, the offer {offer.IdjobVacancy} already exists.";
                _logger.LogError(error);
                return OfferModificationResult.Failure(new List<string> { error });
            }

            CityValidation(offer);
            ValidateJobTitle(ref offer);
            var entity = _mapper.Map(offer, job);
            var company = _enterpriseRepository.Get(offer.Identerprise);
            var companyStatus = company.Idstatus;
            entity.Idstatus = companyStatus == (int)EnterpriseStatus.Active ? (int)OfferStatus.Active : (int)OfferStatus.Pending;
            entity.IntegrationId = offer.IntegrationData.IDIntegration;
            entity.Idsite = company?.SiteId ?? (int)Sites.SPAIN;
            //ADD VACANCY
            if(entity.TitleId > 0)
                entity.Idarea = _denominationsRepository.GetAreaByJobTitle((int)entity.TitleId);


            //--------******----------
            jobVacancyId = _offerRepo.Add(entity);
            //--------******----------


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

            try
            {
                if (offer.InternationalDiffusion.HasValue && offer.InternationalDiffusion.Value)
                {
                    await _internationalDiffusionCountryRepository.RemoveByOffer(jobVacancyId);
                    if (offer.InternationalDiffusionCountries?.Count > 0)
                    {
                        List<InternationalDiffusionCountry> listCountries = new List<InternationalDiffusionCountry>();
                        foreach (int internationalDiffusionCountry in offer.InternationalDiffusionCountries)
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
                if (offer.QuestDTO != null && offer.Idquest == null)
                {
                    //Create a new Questionnaire if none set.
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
                        error = "Failed to create questionnaire";
                        _logger.LogError(error);
                        //Not breaking error, so don't fail here.
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

        /// <summary>
        /// Confirms we have a job title ID and Denomination ID - if not, uses the rest of the info in the job to add one.
        /// </summary>
        /// <param name="offer"></param>
        private void ValidateJobTitle(ref CreateOfferCommand offer)
        {

            if (offer.TitleDenominationId > 0 && offer.TitleId > 0)
                return;

            //Missing Denominations will be set to the default for the JobTitle, if set
            if (offer.TitleDenominationId <= 0 && offer.TitleId > 0)
            {
                offer.TitleDenominationId = _denominationsRepository.GetDefaultDenomination(offer.TitleId, offer.Idsite).FkJobTitle;
                return;
            }

            //Otherwise we'll set the Job title by asking ChatGPT.
            //Get the list of denominations that match the Area we have
            var denominationsForArea = _denominationsRepository.GetAllForArea(offer.Idarea);

            if (!denominationsForArea.Any())
            {
                _logger.LogError($"No Denominations for the given Area {offer.Idarea}, Title: {offer.Title}");
                offer.TitleDenominationId = -1;
                offer.TitleId = -1;
                return;
            }

            //Give them all to ChatGPT along with our TitleString, and make it pick.
            var denominationListString = MakeDenominationList(denominationsForArea);
            var gptResult = _aiService.DoGPTRequestDynamic(denominationListString, offer.Title);
            
            var selectedValue = denominationsForArea.FirstOrDefault(d => d.Id == gptResult);
            if (selectedValue != null)
            {
                offer.TitleDenominationId = selectedValue.Id;
                offer.TitleId = selectedValue.FkJobTitle;
                return;
            }
            
            _logger.LogError($"JobTitleDenomination Failed to find match when posting job. GPT Result: {gptResult}, Title: {offer.Title}");
            offer.TitleDenominationId = -1;
            offer.TitleId = -1;
            return;            
        }

        private string MakeDenominationList(List<JobTitleDenomination> denominationsForArea)
        {
            return denominationsForArea.Select(d => $"{d.Id} : {d.Denomination}").Aggregate((a, b) => a + $", {b}");
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
