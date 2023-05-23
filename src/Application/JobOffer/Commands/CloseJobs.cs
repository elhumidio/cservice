using Application.Aimwel.Commands;
using Application.Aimwel.Interfaces;
using Application.Aimwel.Queries;
using Application.JobOffer.DTO;
using AutoMapper;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.JobOffer.Commands
{
    public class CloseJobs
    {
        public class Command : IRequest<OfferModificationResult>
        {
            public JobClosingReasonDto dto { get; set; }
        }

        public class Handler : IRequestHandler<Command, OfferModificationResult>
        {
            private readonly IJobOfferRepository _offerRepo;
            private readonly IRegEnterpriseContractRepository _regEnterpriseContractRepository;
            private readonly IAimwelCampaign _manageCampaign;
            private readonly IConfiguration _config;
            private readonly IContractProductRepository _contractProductRepo;
            private readonly IMediator _mediatr;
            private readonly IJobVacancyLanguageRepository _jobVacancyLanguageRepo;
            private readonly IRegJobVacWorkPermitRepository _regJobVacWorkPermitRepo;
            private readonly IMapper _mapper;

            public Handler(IJobOfferRepository offerRepo,
                IRegEnterpriseContractRepository regEnterpriseContractRepository,
                IAimwelCampaign aimwelCampaign,
                IConfiguration config,
                IContractProductRepository contractProductRepo, IMediator mediatr,
                IJobVacancyLanguageRepository jobVacancyLanguageRepo,
                IRegJobVacWorkPermitRepository regJobVacWorkPermitRepo,
                IMapper mapper
                )
            {
                _offerRepo = offerRepo;
                _regEnterpriseContractRepository = regEnterpriseContractRepository;
                _manageCampaign = aimwelCampaign;
                _config = config;
                _contractProductRepo = contractProductRepo;
                _mediatr = mediatr;
                _jobVacancyLanguageRepo = jobVacancyLanguageRepo;
                _regJobVacWorkPermitRepo = regJobVacWorkPermitRepo;
                _mapper = mapper;
            }

            public async Task<OfferModificationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                string msg = string.Empty;
                var job = _offerRepo.GetOfferById(request.dto.id);

                bool aimwelEnabled = Convert.ToBoolean(_config["Aimwel:EnableAimwel"]);
                int[] aimwelEnabledSites = _config["Aimwel:EnabledSites"].Split(',').Select(h => Int32.Parse(h)).ToArray();
                aimwelEnabled = aimwelEnabled && aimwelEnabledSites.Contains(job.Idsite);


                if (job == null)
                {
                    return OfferModificationResult.Failure(new List<string> { msg });
                }

             //   await _manageCampaign.StopCampaign(job.IdjobVacancy);
                job.IdClosingReason = request.dto.ClosingReasonId;
                await _offerRepo.UpdateOffer(job);
                var ret = _offerRepo.FileOffer(job);

                if (ret <= 0)
                {
                    msg += $"Offer {request.dto.id} - Not Filed ";
                    return OfferModificationResult.Failure(new List<string> { msg });
                }
                else
                {
                    var langs = _jobVacancyLanguageRepo.Get(job.IdjobVacancy);
                    if (langs != null && langs.Any())
                        _jobVacancyLanguageRepo.Delete(job.IdjobVacancy);
                    var ans = await _regJobVacWorkPermitRepo.Delete(job.IdjobVacancy);
                    var isPack = _contractProductRepo.IsPack(job.Idcontract);
                    if (isPack)
                        await _regEnterpriseContractRepository.IncrementAvailableUnits(job.Idcontract, job.IdjobVacType);
                    msg += $"Filed offer {request.dto.id}\n\r";

                    if (aimwelEnabled)
                    {
                        var campaign = await _mediatr.Send(new GetStatus.Query
                        {
                            OfferId = request.dto.id
                        });

                        await _manageCampaign.StopCampaign(job);
                        msg += $"Campaign {campaign.CampaignId} /  {request.dto.id} - Canceled ";
                        
                    }
                    OfferResultDto dto = new OfferResultDto();
                    dto = _mapper.Map(job, dto);
                    return OfferModificationResult.Success(dto);
                }
            }
        }
    }
}
