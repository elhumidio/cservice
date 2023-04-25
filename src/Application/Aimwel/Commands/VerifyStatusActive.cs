using Application.Aimwel.Interfaces;
using Application.Core;
using Application.Interfaces;
using Domain.DTO;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Aimwel.Commands
{
    public class VerifyStatusActive
    {
        public class Query : IRequest<Result<bool>>
        {
        }

        //TODO divide logic in two parts, actives and inactives

        public class Handler : IRequestHandler<Query, Result<bool>>
        {
            private readonly IAimwelCampaign _manageCampaign;
            private readonly IJobOfferRepository _jobOfferRepository;
            private readonly ICampaignsManagementRepository _campaignsManagementRepository;
            private readonly IApplicationService _applicationService;
            private readonly ILogger<Handler> _logger;

            public Handler(IAimwelCampaign aimwelCampaign,
                IJobOfferRepository jobOfferRepository,
                ICampaignsManagementRepository campaignsManagementRepository,
                IApplicationService applicationService, ILogger<Handler> logger)
            {
                _manageCampaign = aimwelCampaign;
                _jobOfferRepository = jobOfferRepository;
                _campaignsManagementRepository = campaignsManagementRepository;
                _applicationService = applicationService;
                _logger = logger;
            }

            public async Task<Result<bool>> Handle(Query request, CancellationToken cancellationToken)
            {
                var offers = await _jobOfferRepository.GetOffersCreatedLastTwoDays();
                var inactiveOffers = await _jobOfferRepository.GetInactiveOffersChunk();

                ListOffersRequest offersIdsList = new ListOffersRequest()
                {
                    Offers = offers.Select(o => o.IdjobVacancy).ToList(),
                };
                var applicants = await _applicationService.CountApplicantsByOffers(offersIdsList);
                var redirects = await _applicationService.CountRedirectsByOffer(offersIdsList);

                foreach (var offer in offers)
                {
                    var campaign = await _manageCampaign.GetCampaignState(offer.IdjobVacancy);
                    CampaignSetting setting = new();

                    bool isRedirect = offer.ExternalUrl != null;
                    setting = await _campaignsManagementRepository.GetCampaignSetting(offer);
                    if (setting == null)
                    {
                        setting = new CampaignSetting();
                        setting.Goal = 100;
                        setting.Budget = 0.000m;
                    }
                    var goal = setting.Goal;
                    var applicationsList = applicants.results.Where(o => o.jobId == offer.IdjobVacancy).ToList();
                    var redirectionsList = redirects.results.Where(o => o.jobId == offer.IdjobVacancy).ToList();
                    var applications = applicationsList.Any() ? applicationsList.FirstOrDefault().Applicants : 0;
                    var redirections = redirectionsList.Any() ? redirectionsList.FirstOrDefault().Applicants : 0;

                    if (isRedirect)
                    {
                        if (redirections >= goal)
                        {
                            _logger.LogInformation("CANCEL CAMPAIGN - GOAL REACHED", new { campaign.CampaignId, offer.IdjobVacancy });
                            await _manageCampaign.StopCampaign(offer.IdjobVacancy);
                        }
                        else
                        {
                            var mustResume = campaign != null && campaign.Status == DPGRecruitmentCampaignClient.CampaignStatus.Paused;
                            var mustCreate = (campaign != null && campaign.Status == DPGRecruitmentCampaignClient.CampaignStatus.Ended)
                                    || campaign is null;

                            if (mustResume)
                            {
                                await _manageCampaign.ResumeCampaign(offer.IdjobVacancy);
                                _logger.LogInformation("RESUME CAMPAIGN - GOAL NOT REACHED YET", new { campaign.CampaignId, offer.IdjobVacancy });
                            }

                            if (mustCreate)
                            {
                                var campaignCreated = await _manageCampaign.CreateCampaing(offer);
                                _logger.LogInformation("CREATE CAMPAIGN - CAMPAIGN ENDED", new { campaign.CampaignId, CreatedCampaignId = campaignCreated.CampaignId, offer.IdjobVacancy });
                            }
                        }
                    }
                    else
                    {
                        if (applications >= goal)
                        {
                            await _manageCampaign.StopCampaign(offer.IdjobVacancy);
                            _logger.LogInformation("CANCEL CAMPAIGN - GOAL REACHED", new { campaign.CampaignId, offer.IdjobVacancy });
                        }
                        else
                        {
                            var mustResume = campaign != null && campaign.Status == DPGRecruitmentCampaignClient.CampaignStatus.Paused;
                            var mustCreate = (campaign != null && campaign.Status == DPGRecruitmentCampaignClient.CampaignStatus.Ended)
                               || campaign is null;

                            if (mustResume)
                            {
                                await _manageCampaign.ResumeCampaign(offer.IdjobVacancy);
                                _logger.LogInformation("RESUME CAMPAIGN - GOAL NOT REACHED", new { campaign.CampaignId, offer.IdjobVacancy });
                            }
                            if (mustCreate)
                            {
                                var campaignCreated = await _manageCampaign.CreateCampaing(offer);
                                _logger.LogInformation("CREATE CAMPAIGN - GOAL NOT REACHED", new { campaign.CampaignId, CreatedCampaignId = campaignCreated.CampaignId, offer.IdjobVacancy });
                            }
                        }
                    }
                }

                return Result<bool>.Success(true);
            }
        }
    }
}
