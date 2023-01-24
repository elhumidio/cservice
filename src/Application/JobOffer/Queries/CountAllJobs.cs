using Application.Core;
using Domain.Classes;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class CountAllJobs
    {
        public class Query : IRequest<Result<int>>
        {
            //public int DaysOld { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
            private readonly IJobOfferRepository _jobOffer;

            public Handler(IJobOfferRepository jobOffer)
            {
                _jobOffer = jobOffer;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<int>.Success(await _jobOffer.CountAllJobs());
            }
        }
    }
}
