using Application.Core;
using Domain.Classes;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class ListActiveJobsByIds
    {
        public class Query : IRequest<Result<List<int>?>>
        {
            public List<int>? OffersIds { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<int>?>>
        {
            private readonly IJobOfferRepository _jobOffer;

            public Handler(IJobOfferRepository jobOffer)
            {
                _jobOffer = jobOffer;
            }

            public async Task<Result<List<int>?>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<int>? finalOffersIds = await _jobOffer.GetActiveJobsByIds(request.OffersIds);
                return Result<List<int>?>.Success(finalOffersIds);
            }
        }
    }
}
