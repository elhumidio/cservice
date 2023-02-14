using Application.Core;
using Domain.Classes;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class ListActiveJobs
    {
        public class Query : IRequest<Result<IReadOnlyList<JobDataDefinition>>>
        {
            //public int DaysOld { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<IReadOnlyList<JobDataDefinition>>>
        {
            private readonly IJobOfferRepository _jobOffer;

            public Handler(IJobOfferRepository jobOffer)
            {
                _jobOffer = jobOffer;
            }

            public async Task<Result<IReadOnlyList<JobDataDefinition>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<IReadOnlyList<JobDataDefinition>>.Success(await _jobOffer.GetActiveJobs());
            }
        }
    }
}
