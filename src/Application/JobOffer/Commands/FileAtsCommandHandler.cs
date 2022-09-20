using Application.JobOffer.DTO;
using Application.JobOffer.Queries;
using AutoMapper;
using Domain.Enums;
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
        private readonly IMapper _mapper;

        public FileAtsCommandHandler(IJobOfferRepository jobOfferRepository,
            IRegJobVacMatchingRepository regJobVacMatchingRepository,
            IRegEnterpriseContractRepository regEnterpriseContractRepository,
            IContractProductRepository contractProductRepo,
            ILogger<FileAtsCommandHandler> logger,
            IMediator mediatr, IMapper mapper)
        {
            _regJobVacRepo = regJobVacMatchingRepository;
            _offerRepo = jobOfferRepository;
            _logger = logger;
            _regEnterpriseContractRepository = regEnterpriseContractRepository;
            _contractProductRepo = contractProductRepo;
            _mediatr = mediatr;
            _mapper = mapper;
        }

        public async Task<OfferModificationResult> Handle(FileAtsOfferCommand offer, CancellationToken cancellationToken)
        {
            var ats = await _regJobVacRepo.GetAtsIntegrationInfoForFile(offer.Application_reference);
            var offerToFile = ats.FirstOrDefault();
            var jobToUpdateUnits = _offerRepo.GetOfferById(offerToFile.IdjobVacancy);
            var filed = 0;
            if (ats != null)
            {
                bool isActiveOffer = false;
                foreach (var atsInfo in ats)
                {
                    var job = _offerRepo.GetOfferById(atsInfo.IdjobVacancy);
                    isActiveOffer = !job.ChkFilled && !job.ChkDeleted && job.FinishDate.Date >= DateTime.Now.Date && job.Idstatus == (int)OfferStatus.Active;
                    if (isActiveOffer)
                    {
                        var ret = _offerRepo.FileOffer(job);
                        if (ret > 0) filed++;                      
                    }
                }
                if (filed > 0)
                {
                    bool IsPack = _contractProductRepo.IsPack(jobToUpdateUnits.Idcontract);

                    if (IsPack)
                        await _regEnterpriseContractRepository.IncrementAvailableUnits(jobToUpdateUnits.Idcontract, jobToUpdateUnits.IdjobVacType);
                    var info = $"IDJobVacancy: {jobToUpdateUnits.IdjobVacancy} - IdIntegration: {offer.IDIntegration} - Reference: {offer.Application_reference} - Offer Filed";
                    _logger.LogInformation(info);
                    var offerDto = new OfferResultDto();
                    offerDto = _mapper.Map(jobToUpdateUnits, offerDto);
                    return OfferModificationResult.Success(offerDto);
                }
                else
                {
                    var err = $"IDJobVacancy: {jobToUpdateUnits.IdjobVacancy} - IdIntegration: {offer.IDIntegration} - Reference: {offer.Application_reference} - Failed to file ats offer";
                    _logger.LogError(err);
                    return OfferModificationResult.Failure(new List<string> { err });
                }
            }
            return OfferModificationResult.Failure(new List<string> { "AtsInfo is null" });
        }
    }
}
