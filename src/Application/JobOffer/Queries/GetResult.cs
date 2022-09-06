using Application.JobOffer.DTO;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class GetResult
    {
        public class Query : IRequest<OfferResultDto>
        {
            public string ExternalId { get; set; }
            public int OfferId { get; set; }
        }

        public class Handler : IRequestHandler<Query, OfferResultDto>
        {
            private readonly IJobOfferRepository _jobOffer;
            private readonly IRegJobVacMatchingRepository _regJobVacMatchingRepository;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOffer, IRegJobVacMatchingRepository regJobVacMatchingRepository)
            {
                _mapper = mapper;
                _jobOffer = jobOffer;
                _regJobVacMatchingRepository = regJobVacMatchingRepository;
            }

            public Task<OfferResultDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var offerDto = new OfferResultDto();
                var job = _jobOffer.GetOfferById(request.OfferId);
                var integration = _regJobVacMatchingRepository.GetAtsIntegrationInfo(request.ExternalId).Result;
                offerDto = _mapper.Map(job, offerDto);
                _mapper.Map(integration, offerDto.IntegrationData);
                return Task.FromResult(offerDto);
            }
        }
    }
}
