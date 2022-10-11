using Application.Aimwel.Interfaces;
using Application.Core;
using Application.JobOffer.DTO;
using Domain.Repositories;
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
            private readonly IContractProductRepository _contractProductRepo;

            public Handler(IJobOfferRepository offerRepo,
                IRegEnterpriseContractRepository regEnterpriseContractRepository,
                IAimwelCampaign aimwelCampaign,
                IConfiguration config,
                IContractProductRepository contractProductRepo)
            {
                _offerRepo = offerRepo;
                _regEnterpriseContractRepository = regEnterpriseContractRepository;
                _manageCampaign = aimwelCampaign;
                _config = config;
                _contractProductRepo = contractProductRepo;
            }

            public async Task<OfferModificationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                string msg = string.Empty;
                
                    var job = _offerRepo.GetOfferById(request.id);
                    if (job != null)
                    {
                        //if (Convert.ToBoolean(_config["Aimwel:EnableAimwel"]))
                        await _manageCampaign.PauseCampaign(job.IdjobVacancy);
                        var ret = _offerRepo.FileOffer(job);
                        if (ret > 0)
                        {
                            var isPack = _contractProductRepo.IsPack(job.Idcontract);
                            await _regEnterpriseContractRepository.IncrementAvailableUnits(job.Idcontract, job.IdjobVacType);
                            msg += $"Failed to file offer {request.id}\n\r";
                        }
                        else msg += $"Offer {request.id} - Filed Successfully ";
                    }
                
                return OfferModificationResult.Success(new List<string> { msg });
            }
        }
    }
}
