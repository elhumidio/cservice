using Application.Core;
using Domain.Classes;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class GetActiveJobsFollowedCompaniesSinceLastLogin
    {
        public class Get : IRequest<Result<IReadOnlyList<JobDataDefinition>>>
        {
            public string? LastLoggin { get; set; }
            public int[] FollowedCompanies { get; set; }
        }

        public class Handler : IRequestHandler<Get, Result<IReadOnlyList<JobDataDefinition>>>
        {
            private readonly IJobOfferRepository _jobOffer;

            public Handler(IJobOfferRepository jobOffer)
            {
                _jobOffer = jobOffer;
            }

            public async Task<Result<IReadOnlyList<JobDataDefinition>>> Handle(Get request, CancellationToken cancellationToken)
            {
                DateTime _lastLoggin = DateTime.UtcNow;
                DateTime.TryParse(request.LastLoggin, out _lastLoggin);
                if(!request.FollowedCompanies.Any())
                {
                    return Result<IReadOnlyList<JobDataDefinition>>.Failure("Following 0 companies");
                }
                return Result<IReadOnlyList<JobDataDefinition>>.Success(await _jobOffer.GetActiveJobsSinceADate(_lastLoggin, request.FollowedCompanies));
            }
        }
    }
}
