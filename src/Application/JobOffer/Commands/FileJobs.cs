using Application.Aimwel.Interfaces;
using Application.Aimwel.Queries;
using Application.JobOffer.DTO;
using AutoMapper;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.JobOffer.Commands
{
    public class FileJobs
    {
        public class Command : IRequest<OfferModificationResult>
        {
            public int id { get; set; }
        }

        public class Handler : IRequestHandler<Command, OfferModificationResult>
        {
            private readonly IJobOfferRepository _offerRepo;
            private readonly IRegEnterpriseContractRepository _regEnterpriseContractRepository;
            private readonly IAimwelCampaign _manageCampaign;
            private readonly IConfiguration _config;
            private readonly IMediator _mediatr;
            private readonly IContractProductRepository _contractProductRepo;
            private readonly IMapper _mapper;            

            public Handler(IJobOfferRepository offerRepo,
                IRegEnterpriseContractRepository regEnterpriseContractRepository,
                IAimwelCampaign aimwelCampaign,
                IConfiguration config,
                IContractProductRepository contractProductRepo,
                IMediator mediatr,IMapper mapper)
            {
                _offerRepo = offerRepo;
                _regEnterpriseContractRepository = regEnterpriseContractRepository;
                _manageCampaign = aimwelCampaign;
                _config = config;
                _contractProductRepo = contractProductRepo;
                _mediatr = mediatr;
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
                    return OfferModificationResult.Success( new List<string> { msg });
                }
                var ret = _offerRepo.FileOffer(job);
                if (ret <= 0)
                {
                    msg += $"Offer {request.id} - Couldn't file job";                    
                    return OfferModificationResult.Success(new List<string> { msg });
                }
                else
                {
                    var isPack = _contractProductRepo.IsPack(job.Idcontract);
                    if (isPack)
                        await _regEnterpriseContractRepository.IncrementAvailableUnits(job.Idcontract, job.IdjobVacType);
                    msg += $"Offer {request.id} filed.\n\r";                    

                    if (aimwelEnabled)
                    {
                        var campaign = await _mediatr.Send(new GetStatus.Query
                        {
                            OfferId = request.id
                        });
                        if (campaign != null && campaign.Status == CampaignStatus.Active)
                        {
                            await _manageCampaign.StopCampaign(job.IdjobVacancy);
                            msg += $"Campaign {campaign.CampaignId} /  {request.id} - Canceled ";                    
                        }
                        else if(campaign != null)
                        {
                            msg += $"Campaign {campaign.CampaignId} not editable  /  {campaign.Status}";                            
                        }
                    }
                }
                OfferResultDto dto = new OfferResultDto();
                dto = _mapper.Map(job, dto);
                return OfferModificationResult.Success(dto);
            }
        }
    }
}
