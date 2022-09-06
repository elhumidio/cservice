using Application.JobOffer.DTO;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class Get
    {
        public class Query : IRequest<JobOfferDto>
        {
            public int OfferId { get; set; }
        }

        public class Handler : IRequestHandler<Query, JobOfferDto>
        {
            private readonly IJobOfferRepository _jobOffer;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOffer)
            {
                _mapper = mapper;
                _jobOffer = jobOffer;
            }

            public Task<JobOfferDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var jobdto = new JobOfferDto();
                var job = _jobOffer.GetOfferById(request.OfferId);
                _mapper.Map(job, jobdto);
                return Task.FromResult(jobdto);
            }
        }
    }
}
