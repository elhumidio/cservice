using Application.Aimwel.Commands;
using Application.Aimwel.Interfaces;
using Application.Aimwel.Queries;
using Application.JobOffer.DTO;
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

            public Handler(IJobOfferRepository offerRepo,
                IRegEnterpriseContractRepository regEnterpriseContractRepository,
                IAimwelCampaign aimwelCampaign,
                IConfiguration config,
                IContractProductRepository contractProductRepo, IMediator mediatr)
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
                string msg = string.Empty;
                bool aimwelEnabled = Convert.ToBoolean(_config["Aimwel:EnableAimwel"]);

                var job = _offerRepo.GetOfferById(request.dto.id);

                if (job == null)
                {
                    return OfferModificationResult.Success(new List<string> { msg });
                }

                await _manageCampaign.PauseCampaign(job.IdjobVacancy);
                job.IdClosingReason = request.dto.ClosingReasonId;
                await _offerRepo.UpdateOffer(job);
                var ret = _offerRepo.FileOffer(job);

                if (ret <= 0)
                    msg += $"Offer {request.dto.id} - Not Filed ";
                else
                {
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

                        if (campaign != null && campaign.Status == CampaignStatus.Active)
                        {
                            var ans = _mediatr.Send(new Cancel.Command
                            {
                                offerId = request.dto.id
                            });
                            msg += $"Campaign {campaign.CampaignId} /  {request.dto.id} - Canceled ";
                        }
                    }
                }

                return OfferModificationResult.Success(new List<string> { msg });
            }
        }
    }
}
