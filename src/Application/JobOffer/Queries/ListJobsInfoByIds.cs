using Application.Core;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class ListJobsInfoByIds
    {
        public class Query : IRequest<Result<List<JobVacancy>>>
        {
            public List<int>? OffersIds { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<JobVacancy>>>
        {
            private readonly IJobOfferRepository _jobOffer;

            public Handler(IJobOfferRepository jobOffer)
            {
                _jobOffer = jobOffer;
            }

            public async Task<Result<List<JobVacancy>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<JobVacancy>>.Success(await _jobOffer.GetJobsInfoByIds(request.OffersIds));
            }
        }
    }
}
