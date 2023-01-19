using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class CountOffersPublished
    { 
        public class Query : IRequest<Result<int>>
        {
            public int Days { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
            private readonly IJobOfferRepository _jobOffer;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOffer, IContractProductRepository contractProductRepo)
            {
                _mapper = mapper;
                _jobOffer = jobOffer;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<int>.Success(await _jobOffer.CountOffersPublished(request.Days));
            }
        }
    }
}
