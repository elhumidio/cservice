using Application.JobOffer.DTO;
using Application.Utils;
using AutoMapper;
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
            private readonly IConfiguration _config;
            private readonly IMediator _mediatr;
            private readonly IContractProductRepository _contractProductRepo;
            private readonly IMapper _mapper;
            private readonly IApiUtils _utils;

            public Handler(IJobOfferRepository offerRepo,
                IRegEnterpriseContractRepository regEnterpriseContractRepository,                
                IConfiguration config,
                IContractProductRepository contractProductRepo,
                IMediator mediatr, IMapper mapper, IApiUtils utils)
            {
                _offerRepo = offerRepo;
                _regEnterpriseContractRepository = regEnterpriseContractRepository;                
                _config = config;
                _contractProductRepo = contractProductRepo;
                _mediatr = mediatr;
                _mapper = mapper;
                _utils = utils;
            }

            public async Task<OfferModificationResult> Handle(Command request, CancellationToken cancellationToken)
            {
                string msg = string.Empty;
                var job = _offerRepo.GetOfferById(request.id);

                if (job == null)
                {
                    return OfferModificationResult.Success(new List<string> { msg });
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
                }
                OfferResultDto dto = new OfferResultDto();
                dto = _mapper.Map(job, dto);

                // Google API Indexing URL Delete.
                _utils.DeleteGoogleIndexingURL(_utils.GetOfferModel(job));

                return OfferModificationResult.Success(dto);
            }
        }
    }
}
