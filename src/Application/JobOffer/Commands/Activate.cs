using Application.Aimwel.Commands;
using Application.Aimwel.Interfaces;
using Application.Aimwel.Queries;
using Application.Contracts.Queries;
using Application.JobOffer.DTO;
using Domain.Enums;
using Domain.Repositories;
using DPGRecruitmentCampaignClient;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.JobOffer.Commands
{
    public class Activate : IRequest<OfferModificationResult>
    {
        public class Command : IRequest<OfferModificationResult>
        {
            public int id { get; set; }
            public int OwnerId { get; set; }
        }

        public class Handler : IRequestHandler<Command, OfferModificationResult>
        {
            private readonly IJobOfferRepository _offerRepo;
            private readonly IRegEnterpriseContractRepository _regEnterpriseContractRepository;            
            private readonly IConfiguration _config;
            private readonly IContractProductRepository _contractProductRepo;
            private readonly IMediator _mediatr;
            private readonly ILogger _logger;

            public Handler(IJobOfferRepository offerRepo,
                IRegEnterpriseContractRepository regEnterpriseContractRepository,                
                IConfiguration config,
                IContractProductRepository contractProductRepo,
                IMediator mediatr,
                ILogger logger)
            {
                _offerRepo = offerRepo;
                _regEnterpriseContractRepository = regEnterpriseContractRepository;                
                _config = config;
                _contractProductRepo = contractProductRepo;
                _mediatr = mediatr;
                _logger = logger;
            }

            public async Task<OfferModificationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                string msg = string.Empty;
                bool aimwelEnabled = Convert.ToBoolean(_config["Aimwel:EnableAimwel"]);
                bool offerUpdated = false;
                var job = _offerRepo.GetOfferById(request.id);

                if (job == null)
                {
                    return OfferModificationResult.Success(new List<string> { msg });
                }

                var units = _mediatr.Send(new GetAvailableUnitsByOwner.Query
                {
                    ContractId = job.Idcontract,
                    OwnerId = request.OwnerId
                }).Result.Value;
                bool hasUnits = units != null && units.FirstOrDefault(u => u.type == (VacancyType)job.IdjobVacType).Units > 0;

                if (!hasUnits)
                {
                    msg += "not_enough_units";
                    _logger.LogError(msg);
                    return OfferModificationResult.Success(new List<string> { msg });
                }
                else
                {
                    job.FilledDate = null;
                    job.ChkFilled = false;
                    job.ChkDeleted = false;
                    job.ModificationDate = DateTime.Now;
                    var ret = await _offerRepo.UpdateOffer(job);

                    var isPack = _contractProductRepo.IsPack(job.Idcontract);
                    if (isPack)
                        await _regEnterpriseContractRepository.UpdateUnits(job.Idcontract, job.IdjobVacType);
                    msg += $"Activated offer {request.id}";
                    _logger.LogInformation(msg);    
                    offerUpdated = true;
                }
      
                bool canModifyCampaign = aimwelEnabled && offerUpdated;

                if (canModifyCampaign)
                {

                    var campaign = await _mediatr.Send(new GetStatus.Query
                    {
                        OfferId = request.id
                    });

                    if (campaign != null && campaign.Status == CampaignStatus.Paused)
                    {
                        var ans = _mediatr.Send(new Resume.Command
                        {
                            offerId = request.id
                        });
                        msg += $"Campaign: {campaign.CampaignId} /  id: {request.id} - resumed ";
                        _logger.LogInformation(msg);
                    }
                    else if (campaign != null && campaign.Status == CampaignStatus.Ended)
                    {
                        var ans = _mediatr.Send(new Create.Command
                        {
                            offerId = request.id
                        });
                        msg += $"Campaign: {ans.Result.Value.CampaignId} /  id: {request.id} - created ";
                        _logger.LogInformation(msg);
                    }
                    return OfferModificationResult.Success(new List<string> { msg });
                }
                else
                {
                    return OfferModificationResult.Success(new List<string> { msg });
                }
                
            }
        }
    }
}
