using Application.Core;
using Domain.Classes;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class ListAllJobsPaged
    {
        public class Query : IRequest<Result<List<JobData>>>
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<JobData>>>
        {
            private readonly IJobOfferRepository _jobOffer;

            public Handler(IJobOfferRepository jobOffer)
            {
                _jobOffer = jobOffer;
            }

            public async Task<Result<List<JobData>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<JobData>>.Success(await _jobOffer.ListAllJobsPaged(request.Page, request.PageSize));
            }
        }
    }
}
