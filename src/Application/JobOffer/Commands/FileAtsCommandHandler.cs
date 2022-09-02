using Application.Core;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.JobOffer.Commands
{
    public class FileAtsCommandHandler : IRequestHandler<FileAtsOfferCommand, Result<Unit>>
    {
        private readonly IJobOfferRepository _offerRepo;
        private readonly IRegEnterpriseContractRepository _regEnterpriseContractRepository;
        private readonly IRegJobVacMatchingRepository _regJobVacRepo;
        private readonly ILogger<FileAtsCommandHandler> _logger;

        public FileAtsCommandHandler(IJobOfferRepository jobOfferRepository,
            IRegJobVacMatchingRepository regJobVacMatchingRepository,
            IRegEnterpriseContractRepository regEnterpriseContractRepository,
            ILogger<FileAtsCommandHandler> logger)
        {
            _regJobVacRepo = regJobVacMatchingRepository;
            _offerRepo = jobOfferRepository;
            _logger = logger;
            _regEnterpriseContractRepository = regEnterpriseContractRepository; 
        }

        public async Task<Result<Unit>> Handle(FileAtsOfferCommand offer, CancellationToken cancellationToken)
        {
            var atsInfo = await _regJobVacRepo.GetAtsIntegrationInfo(offer.Application_reference);
            if (atsInfo != null)
            {
                var job = _offerRepo.GetOfferById(atsInfo.IdjobVacancy);
                var filed = _offerRepo.FileOffer(job);
                if (filed.Result > 0) {
                    
                    await _regEnterpriseContractRepository.ReduceUnits(job.Idcontract, job.IdjobVacType);
                    return Result<Unit>.Success(Unit.Value);
                }   
                else
                {
                    _logger.LogError($"idjobvacancy: {job.IdjobVacancy} - IdIntegration: {offer.IDIntegration} - Reference: {offer.Application_reference} - Failed to file ats offer");
                    return Result<Unit>.Failure("Failed to file offer");
                }
            }
            return Result<Unit>.Failure("Failed to file ats offer");
        }
    }
}
