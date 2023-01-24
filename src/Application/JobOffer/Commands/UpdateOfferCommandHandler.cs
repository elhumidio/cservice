using Application.Aimwel.Commands;
using Application.Aimwel.Queries;
using Application.DTO;
using Application.EnterpriseContract.Queries;
using Application.JobOffer.DTO;
using Application.JobOffer.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;
using Microsoft.Extensions.Configuration;
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
        private readonly IRegJobVacWorkPermitRepository _regJobVacWorkPermitRepo;
        private readonly ICityRepository _cityRepository;
        private readonly IConfiguration _config;
        private readonly IEnterpriseRepository _enterpriseRepository;

        #endregion PRIVATE PROPERTIES

        public UpdateOfferCommandHandler(IRegEnterpriseContractRepository regContractRepo,
            IRegJobVacMatchingRepository regJobVacRepo,
            IMapper mapper,
            IJobOfferRepository offerRepo,           
            IContractProductRepository contractProductRepo,         
            IMediator mediatr,
            IRegJobVacWorkPermitRepository regJobVacWorkPermitRepo,
            ICityRepository cityRepository,
            IConfiguration config,
            IEnterpriseRepository enterpriseRepository)
        {
            _contractProductRepo = contractProductRepo;
            _offerRepo = offerRepo;
            _regContractRepo = regContractRepo;
            _regJobVacRepo = regJobVacRepo;
            _mediatr = mediatr;
            _mapper = mapper;
            _regJobVacWorkPermitRepo = regJobVacWorkPermitRepo;
            _cityRepository = cityRepository;
            _config = config;
            _enterpriseRepository = enterpriseRepository;
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
                await ActivateActions(offer, existentOffer);
            }
            
            if (integrationInfo == null)
            {
                offer.Idcity = existentOffer.Idcity;
                offer.Idregion = existentOffer.Idregion;
                offer.Idcountry = existentOffer.Idcountry;
                offer.City = existentOffer.City;
                offer.IdzipCode= existentOffer.IdzipCode;
            }
            var entity = _mapper.Map(offer, existentOffer);
            CityValidation(offer);
            var company = _enterpriseRepository.Get(offer.Identerprise);

            if (company.Idstatus == (int)EnterpriseStatus.Pending)
            {
                offer.Idstatus = (int)EnterpriseStatus.Pending;
            }
            var ret = await _offerRepo.UpdateOffer(existentOffer);
            var updatedOffer = _mediatr.Send(new GetResult.Query
            {
                ExternalId = offer.IntegrationData.ApplicationReference,
                OfferId = offer.IdjobVacancy
            }).Result;

            bool canSaveWorkPermit = offer.IdworkPermit != null && offer.IdworkPermit.Any()
                && integrationInfo == null;
            
            if (canSaveWorkPermit)
            {            
                await _regJobVacWorkPermitRepo.Delete(offer.IdjobVacancy);
                foreach (var permit in offer.IdworkPermit)
                {
                   await _regJobVacWorkPermitRepo.Add(new RegJobVacWorkPermit() { IdjobVacancy = offer.IdjobVacancy, IdworkPermit = permit });
                }
            }

            var integration = _regJobVacRepo.GetAtsIntegrationInfoByJobId(existentOffer.IdjobVacancy).Result;

            if (integration != null)
            {
                IntegrationActions(updatedOffer, integration);
            }

            if (ret < 0)
                return OfferModificationResult.Failure(new List<string> { "no update" });
            else
                return OfferModificationResult.Success(updatedOffer);
        }

        /// <summary>
        /// It puts city name
        /// </summary>
        /// <param name="offer"></param>
        private void CityValidation(UpdateOfferCommand offer)
        {
            if (string.IsNullOrEmpty(offer.City.Trim()))
            {
                offer.City = string.Empty;
            }

        }

        private static void IntegrationActions(OfferResultDto updatedOffer, RegJobVacMatching integration)
        {
            updatedOffer.IntegrationData.ApplicationEmail = integration.AppEmail;
            updatedOffer.IntegrationData.ApplicationReference = integration.ExternalId;
            updatedOffer.IntegrationData.ApplicationUrl = integration.Redirection;
            updatedOffer.IntegrationData.IDIntegration = integration.Idintegration;
        }

        private async Task ActivateActions(UpdateOfferCommand offer, JobVacancy existentOffer)
        {
            var result = await _mediatr.Send(new GetContract.Query
            {
                CompanyId = offer.Identerprise,
                type = (VacancyType)offer.IdjobVacType,
                RegionId = offer.Idregion
            });
            bool canActivate = result.Value != null && result.Value.Idcontract > 0;
            await ActivateActions(offer, existentOffer, result, canActivate);
        }


        /// <summary>
        /// It perform actions regarding activate offers
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="existentOffer"></param>
        /// <param name="result"></param>
        /// <param name="canActivate"></param>
        /// <returns></returns>
        private async Task ActivateActions(UpdateOfferCommand offer, JobVacancy existentOffer, ContractResult result, bool canActivate)
        {
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
                bool aimwelEnabled = Convert.ToBoolean(_config["Aimwel:EnableAimwel"]);
                int[] aimwelEnabledSites = _config["Aimwel:EnabledSites"].Split(',').Select(h => Int32.Parse(h)).ToArray();
                aimwelEnabled = aimwelEnabled && aimwelEnabledSites.Contains(offer.Idsite);

                if (aimwelEnabled) {
                    var campaign = await _mediatr.Send(new GetStatus.Query
                    {
                        OfferId = offer.IdjobVacancy
                    });

                    if (campaign != null && campaign.Status == CampaignStatus.Paused)
                    {
                        var ans = _mediatr.Send(new Resume.Command
                        {
                            offerId = offer.IdjobVacancy
                        });
                        
                    }
                    else if (campaign != null && campaign.Status == CampaignStatus.Ended)
                    {
                        var ans = _mediatr.Send(new Create.Command
                        {
                            offerId = offer.IdjobVacancy
                        });                        
                    }
                }

                await _regContractRepo.UpdateUnits(result.Value.Idcontract, (int)result.Value.IdJobVacType);
            }
            else
            {
                offer.FilledDate = existentOffer.FilledDate;
                offer.ChkUpdateDate = existentOffer.ChkUpdateDate;
                offer.ChkFilled = existentOffer.ChkFilled;
                offer.ChkDeleted = existentOffer.ChkDeleted;
                offer.IdjobVacType = existentOffer.IdjobVacType;
                offer.PublicationDate = existentOffer.PublicationDate;
                offer.UpdatingDate = DateTime.Now;
                offer.Idstatus = existentOffer.Idstatus;
            }
        }
    }
}
