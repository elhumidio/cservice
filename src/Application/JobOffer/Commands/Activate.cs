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

            public Handler(IJobOfferRepository offerRepo,
                IRegEnterpriseContractRepository regEnterpriseContractRepository,                
                IConfiguration config,
                IContractProductRepository contractProductRepo, IMediator mediatr)
            {
                _offerRepo = offerRepo;
                _regEnterpriseContractRepository = regEnterpriseContractRepository;                
                _config = config;
                _contractProductRepo = contractProductRepo;
                _mediatr = mediatr;
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
                bool hasUnits = units != null && units.Where(u => u.type == (VacancyType)job.IdjobVacType).FirstOrDefault().Units > 0;

                if (!hasUnits)
                {
                    msg += "not_enough_units";
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
                    offerUpdated = true;
                }

                if (!aimwelEnabled)
                {
                    return OfferModificationResult.Success(new List<string> { msg });
                }

                if (offerUpdated)
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
                    }
                    else if (campaign != null && campaign.Status == CampaignStatus.Ended)
                    {
                        var ans = _mediatr.Send(new Create.Command
                        {
                            offerId = request.id
                        });
                        msg += $"Campaign: {ans.Result.Value.CampaignId} /  id: {request.id} - created ";
                    }
                }
                return OfferModificationResult.Success(new List<string> { msg });
            }
        }
    }
}
