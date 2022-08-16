using Application.Core;
using Application.JobOffer.DTO;
using Domain.Repositories;
using MediatR;


namespace Application.JobOffer.Commands
{
    public class FileAtsJob : IRequestHandler<FileOfferDto, Result<Unit>>
    {
        private readonly IJobOfferRepository _offerRepo;
        private readonly IRegJobVacMatchingRepository _regJobVacRepo;



        public FileAtsJob(IJobOfferRepository jobOfferRepository, IRegJobVacMatchingRepository regJobVacMatchingRepository)
        {
            _regJobVacRepo = regJobVacMatchingRepository;
            _offerRepo = jobOfferRepository;

        }


        public async Task<Result<Unit>> Handle(FileOfferDto offer, CancellationToken cancellationToken)
        {
            var atsInfo = await _regJobVacRepo.GetAtsIntegrationInfo(offer.Application_reference);
            if (atsInfo != null)
            {
                var job = _offerRepo.GetOfferById(atsInfo.IdjobVacancy);
                var filed = _offerRepo.FileOffer(job);
                if (filed)
                    return Result<Unit>.Success(Unit.Value);
                else return Result<Unit>.Failure("Failed to file offer");
            }
            return Result<Unit>.Failure("Failed to file offer");
        }
    }
}
