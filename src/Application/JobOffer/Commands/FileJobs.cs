using Application.Aimwel.Interfaces;
using Application.Aimwel.Queries;
using Application.JobOffer.DTO;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;
using Microsoft.Extensions.Configuration;

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

            public Handler(IJobOfferRepository offerRepo,
                IRegEnterpriseContractRepository regEnterpriseContractRepository,
                IAimwelCampaign aimwelCampaign,
                IConfiguration config,
                IContractProductRepository contractProductRepo,
                IMediator mediatr)
            {
                _offerRepo = offerRepo;
                _regEnterpriseContractRepository = regEnterpriseContractRepository;
                _manageCampaign = aimwelCampaign;
                _config = config;
                _contractProductRepo = contractProductRepo;
                _mediatr = mediatr;
            }

            public async Task<OfferModificationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                bool aimwelEnabled = Convert.ToBoolean(_config["Aimwel:EnableAimwel"]);
                string msg = string.Empty;

                var job = _offerRepo.GetOfferById(request.id);
                if (job == null)
                {
                    return OfferModificationResult.Success(new List<string> { msg });
                }
                var ret = _offerRepo.FileOffer(job);
                if (ret <= 0)
                    msg += $"Offer {request.id} - Filed Successfully ";
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
                            await _manageCampaign.PauseCampaign(job.IdjobVacancy);
                            msg += $"Campaign {campaign.CampaignId} /  {request.id} - Canceled ";
                        }
                        else
                        {
                            msg += $"Campaign {campaign.CampaignId} not editable  /  {campaign.Status}";
                        }
                    }
                }

                return OfferModificationResult.Success(new List<string> { msg });
            }
        }
    }
}
