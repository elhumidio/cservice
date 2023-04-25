using Application.Aimwel.Interfaces;
using Application.Interfaces;
using Domain.DTO;
using Domain.Entities;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;
using Newtonsoft.Json;

namespace Application.Aimwel.Commands
{
    public class Updater
    {
        public class Command : IRequest<Response>
        {
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IApplicationService _applicationService;
            private readonly IJobOfferRepository _offerRepo;
            private readonly IAimwelCampaign _manageCampaign;
            private readonly ICampaignsManagementRepository _campaignsManagementRepository;

            public Handler(IJobOfferRepository offerRepo,
                IAimwelCampaign aimwelCampaign,
                ICampaignsManagementRepository campaignsManagementRepository,
                IApplicationService applicationService)
            {
                _offerRepo = offerRepo;
                _manageCampaign = aimwelCampaign;
                _campaignsManagementRepository = campaignsManagementRepository;
                _applicationService = applicationService;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {

                List<CampaignsManagement> campaignsList = new();
                var needUpdate = await _manageCampaign.GetCampaignNeedsUpdate("aimwel");
                if (needUpdate)
                {
                    var offersList = _offerRepo.GetActiveOffers().ToList();
                    ListOffersRequest offersIdsList = new()
                    {
                        Offers = offersList.Select(o => o.IdjobVacancy).ToList(),
                    };
                    var applicants = await _applicationService.CountApplicantsByOffers(offersIdsList);
                    var redirects = await _applicationService.CountRedirectsByOffer(offersIdsList);

                    foreach (JobVacancy offer in offersList)
                    {
                        bool cancelCampaign = await _manageCampaign.StopCampaign(offer.IdjobVacancy);
                        bool isRedirect = offer.ExternalUrl != null;
                        CampaignSetting setting = await _campaignsManagementRepository.GetCampaignSetting(offer);
                        if (setting == null)
                        {
                            setting = new CampaignSetting();
                            setting.Goal = 100;
                            setting.Budget = 0.000m;
                        }
                        var goal = isRedirect ? setting.Goal * 20 : setting.Goal;
                        var applications = isRedirect ? redirects.results.Where(o => o.jobId == offer.IdjobVacancy).Count()
                            : applicants.results.Where(o => o.jobId == offer.IdjobVacancy).Count();
                        if (applications < goal) {
                            var campaignCreated = await _manageCampaign.CreateCampaingUpdater(offer);
                            if (campaignCreated != null && !string.IsNullOrEmpty(campaignCreated.ExternalCampaignId))
                            {
                                campaignsList.Add(campaignCreated);
                            }
                        }
                    }
                    _campaignsManagementRepository.AddRange(campaignsList);
                    bool updateMarked = await _manageCampaign.MarkUpdateCampaign("aimwel");
                }
                return new Response();
            }
        }
    }
}
