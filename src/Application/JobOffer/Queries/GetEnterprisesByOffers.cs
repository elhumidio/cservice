using Application.Core;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class GetEnterprisesByOffers
    {
        public class Get : IRequest<Result<List<int>>>
        {
            public List<int> OfferIds { get; set; }

        }

        public class Handler : IRequestHandler<Get, Result<List<int>>>
        {
            private readonly IJobOfferRepository _jobOfferRepository;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOffer)
            {
                _mapper = mapper;
                _jobOfferRepository = jobOffer;
            }

            public async Task<Result<List<int>>> Handle(Get request, CancellationToken cancellationToken)
            {
                var query = _jobOfferRepository.GetOffersByIds(request.OfferIds);
                if (query == null || query.Count == 0)
                {
                    return new Result<List<int>>();
                }
                return Result<List<int>>.Success(query.Select(x => x.Identerprise).Distinct().ToList());
            }
        }
    }
}
