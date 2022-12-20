using Application.Aimwel.Interfaces;
using Application.Core;
using Application.Interfaces;
using Domain.DTO;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Aimwel.Commands
{
    public class VerifyStatus
    {
        public class Command : IRequest<Result<bool>>
        {
            
        }

        public class Handler : IRequestHandler<Command, Result<bool>>
        {
            private readonly IAimwelCampaign _manageCampaign;
            private readonly IJobOfferRepository _jobOfferRepository;
            private readonly ICampaignsManagementRepository _campaignsManagementRepository;
            private readonly IApplicationService _applicationService;
            private readonly ILogger _logger;


            public Handler(IAimwelCampaign aimwelCampaign,
                IJobOfferRepository jobOfferRepository,
                ICampaignsManagementRepository campaignsManagementRepository,
                IApplicationService applicationService, ILogger logger)
            {
                _manageCampaign = aimwelCampaign;
                _jobOfferRepository = jobOfferRepository;
                _campaignsManagementRepository = campaignsManagementRepository;
                _applicationService = applicationService;
                _logger = logger;
            }

            public async Task<Result<bool>> Handle(Command request, CancellationToken cancellationToken)
            {
                var offers = await _jobOfferRepository.GetOffersCreatedLastFortnight();
                
                ListOffersRequest offersIdsList = new ListOffersRequest() {
                     Offers = offers.Select(o => o.IdjobVacancy).ToList(),
                };
                var applicants = await _applicationService.CountApplicantsByOffers(offersIdsList);
                var redirects = await _applicationService.CountRedirectsByOffer(offersIdsList);
                foreach (var offer in offers)
                {
                    
                    var campaign = await _manageCampaign.GetCampaignState(offer.IdjobVacancy);
                    bool active = !offer.ChkDeleted && !offer.ChkFilled && offer.FinishDate >= DateTime.Now.Date;

                    if (active)
                    {
                        bool isRedirect = offer.ExternalUrl != null;
                        if (isRedirect) {
                            //get goal
                            var setting = await _campaignsManagementRepository.GetCampaignSetting(offer);
                            var goal =  setting.Goal * 20;
                            var redirections = redirects.results.Where(o => o.jobId == offer.IdjobVacancy).Count();
                            if (redirections >= goal) {
                                if (campaign.Status == DPGRecruitmentCampaignClient.CampaignStatus.Active)
                                {
                                    //TODO cancel campaign
                                   _manageCampaign.StopCampaign(offer.IdjobVacancy);
                                }
                            }
                            if (redirections < goal) {

                                if (campaign.Status != DPGRecruitmentCampaignClient.CampaignStatus.Active)
                                {
                                    //create or active campaign
                                    if (campaign.Status == DPGRecruitmentCampaignClient.CampaignStatus.Paused)
                                        _manageCampaign.ResumeCampaign(offer.IdjobVacancy);
                                    if(campaign.Status == DPGRecruitmentCampaignClient.CampaignStatus.Ended)
                                        _manageCampaign.CreateCampaing(offer);
                                }
                            }
                        }
                        else {
                            var setting = await _campaignsManagementRepository.GetCampaignSetting(offer);
                            var goal = setting.Goal;
                            var applications = applicants.results.Where(o => o.jobId == offer.IdjobVacancy).Count();
                            if (applications >= goal)
                            {
                                if (campaign.Status == DPGRecruitmentCampaignClient.CampaignStatus.Active)
                                {
                                    _manageCampaign.StopCampaign(offer.IdjobVacancy);
                                }
                            }
                            if (applications < goal) {

                                if (campaign.Status != DPGRecruitmentCampaignClient.CampaignStatus.Active)
                                {
                                    if (campaign.Status != DPGRecruitmentCampaignClient.CampaignStatus.Active)
                                    {
                                        //create or active campaign
                                        if (campaign.Status == DPGRecruitmentCampaignClient.CampaignStatus.Paused)
                                            _manageCampaign.ResumeCampaign(offer.IdjobVacancy);
                                        if (campaign.Status == DPGRecruitmentCampaignClient.CampaignStatus.Ended)
                                            _manageCampaign.CreateCampaing(offer);
                                    }

                                }
                            }
                                
                        }
                    }
                    else
                    {
                        if (campaign.Status == DPGRecruitmentCampaignClient.CampaignStatus.Active) {

                            _manageCampaign.StopCampaign(offer.IdjobVacancy);
                        }

                    }
                    
                }
                return Result<bool>.Success(true);
            }
        }
    }
}
