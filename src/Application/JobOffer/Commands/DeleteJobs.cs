using Application.Aimwel.Interfaces;
using Application.Aimwel.Queries;
using Application.JobOffer.DTO;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.JobOffer.Commands
{
    public class DeleteJobs
    {
        public class Command : IRequest<OfferModificationResult>
        {
            public int id { get; set; }
        }

        public class Handler : IRequestHandler<Command, OfferModificationResult>
        {
            private readonly IJobOfferRepository _offerRepo;
            private readonly IRegEnterpriseContractRepository _regEnterpriseContractRepo;
            private readonly IAimwelCampaign _manageCampaign;
            private readonly IConfiguration _config;
            private readonly IMediator _mediatr;
            private readonly IContractProductRepository _contractProductRepo;
            private readonly IJobVacancyLanguageRepository _jobVacancyLanguageRepo;
            private readonly IRegJobVacWorkPermitRepository _regJobVacWorkPermitRepo;
            private readonly IMapper _mapper;
            

            public Handler(IJobOfferRepository offerRepo,
                IRegEnterpriseContractRepository regEnterpriseContractRepository,
                IAimwelCampaign aimwelCampaign,
                IConfiguration config,
                IContractProductRepository contractProductRepo,
                IMediator mediatr,
                IJobVacancyLanguageRepository jobVacancyLanguageRepository,
                IRegJobVacWorkPermitRepository regJobVacWorkPermitRepo,
                IMapper mapper)
            {
                _offerRepo = offerRepo;
                _regEnterpriseContractRepo = regEnterpriseContractRepository;
                _manageCampaign = aimwelCampaign;
                _config = config;
                _contractProductRepo = contractProductRepo;
                _mediatr = mediatr;
                _jobVacancyLanguageRepo = jobVacancyLanguageRepository;
                _regJobVacWorkPermitRepo = regJobVacWorkPermitRepo;
                _mapper = mapper;                   
            }

            public async Task<OfferModificationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                
                string msg = string.Empty;

                var job = _offerRepo.GetOfferById(request.id);
                bool aimwelEnabled = Convert.ToBoolean(_config["Aimwel:EnableAimwel"]);
                int[] aimwelEnabledSites = _config["Aimwel:EnabledSites"].Split(',').Select(h => Int32.Parse(h)).ToArray();
                aimwelEnabled = aimwelEnabled && aimwelEnabledSites.Contains(job.Idsite);

                if (job == null)
                {
                    return OfferModificationResult.Success(new List<string> { msg });
                }
                var ret = _offerRepo.DeleteOffer(job);
                if (ret <= 0)
                {
                    msg += $"Offer {request.id} - Couldn't delete it";                    
                    return OfferModificationResult.Success(new List<string> { msg });
                }
                else
                {
                    msg += $"Offer {request.id} - Deleted Successfully ";
                    var langs = _jobVacancyLanguageRepo.Get(job.IdjobVacancy);
                    if (langs != null && langs.Any())
                        _jobVacancyLanguageRepo.Delete(job.IdjobVacancy);
                    var ans = await _regJobVacWorkPermitRepo.Delete(job.IdjobVacancy);
                    var isPack = _contractProductRepo.IsPack(job.Idcontract);

                    if (isPack)
                    {
                        await _regEnterpriseContractRepo.IncrementAvailableUnits(job.Idcontract, job.IdjobVacType);                        
                    }

                    if (aimwelEnabled)
                    {
                        var campaign = await _mediatr.Send(new GetStatus.Query
                        {
                            OfferId = request.id
                        });
                        if (campaign != null && campaign.Status != CampaignStatus.Ended)
                        {
                            await _manageCampaign.StopCampaign(job);
                            msg += $"Campaign {campaign.CampaignId} /  {request.id} - Canceled ";                            
                        }
                        else if(campaign != null)
                        {
                            msg += $"Campaign {campaign.CampaignId} not editable  /  {campaign.Status}";                            
                        }
                    }
                }
                OfferResultDto dto = new();
                dto = _mapper.Map(job,dto);
                return OfferModificationResult.Success(dto);
            }
        }
    }
}
