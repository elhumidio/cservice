using Application.JobOffer.DTO;
using Application.JobOffer.Queries;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.JobOffer.Commands
{
    public class FileAtsCommandHandler : IRequestHandler<FileAtsOfferCommand, OfferModificationResult>
    {
        private readonly IJobOfferRepository _offerRepo;
        private readonly IRegEnterpriseContractRepository _regEnterpriseContractRepository;
        private readonly IRegJobVacMatchingRepository _regJobVacRepo;
        private readonly IContractProductRepository _contractProductRepo;
        private readonly ILogger<FileAtsCommandHandler> _logger;
        private readonly IMediator _mediatr;

        public FileAtsCommandHandler(IJobOfferRepository jobOfferRepository,
            IRegJobVacMatchingRepository regJobVacMatchingRepository,
            IRegEnterpriseContractRepository regEnterpriseContractRepository,
            IContractProductRepository contractProductRepo,
            ILogger<FileAtsCommandHandler> logger,
            IMediator mediatr)
        {
            _regJobVacRepo = regJobVacMatchingRepository;
            _offerRepo = jobOfferRepository;
            _logger = logger;
            _regEnterpriseContractRepository = regEnterpriseContractRepository;
            _contractProductRepo = contractProductRepo;
            _mediatr = mediatr;
        }

        public async Task<OfferModificationResult> Handle(FileAtsOfferCommand offer, CancellationToken cancellationToken)
        {
            var atsInfo = await _regJobVacRepo.GetAtsIntegrationInfo(offer.Application_reference);
            if (atsInfo != null)
            {
                var job = _offerRepo.GetOfferById(atsInfo.IdjobVacancy);
                var filed = _offerRepo.FileOffer(job);
                var createdOffer = _mediatr.Send(new GetResult.Query { ExternalId = offer.Application_reference, OfferId = job.IdjobVacancy });
                if (filed.Result > 0)
                {
                    bool IsPack = _contractProductRepo.IsPack(job.Idcontract);

                    if (IsPack)
                        await _regEnterpriseContractRepository.IncrementAvailableUnits(job.Idcontract, job.IdjobVacType);
                    var info = $"IDJobVacancy: {job.IdjobVacancy} - IdIntegration: {offer.IDIntegration} - Reference: {offer.Application_reference} - Offer Filed";
                    _logger.LogInformation(info);
                    return OfferModificationResult.Success(createdOffer.Result);
                }
                else
                {
                    var err = $"IDJobVacancy: {job.IdjobVacancy} - IdIntegration: {offer.IDIntegration} - Reference: {offer.Application_reference} - Failed to file ats offer";
                    _logger.LogError(err);
                    return OfferModificationResult.Failure(new List<string> { err });
                }
            }
            return OfferModificationResult.Failure(new List<string> { "AtsInfo is null" });
        }
    }
}
