using Application.Core;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffers.Queries
{
    public class GetLastPublishedDateByCompaniesIds
    {
        public class Get : IRequest<Result<IReadOnlyList<KeyValueDateTimeDto>>>
        {
            public List<int> CompaniesIds { get; set; }
        }

        public class Handler : IRequestHandler<Get, Result<IReadOnlyList<KeyValueDateTimeDto>>>
        {
            private readonly IJobOfferRepository _jobOfferRepository;

            public Handler(IJobOfferRepository jobOfferRepository)
            {
                _jobOfferRepository = jobOfferRepository;
            }

            public async Task<Result<IReadOnlyList<KeyValueDateTimeDto>>> Handle(Get request, CancellationToken cancellationToken)
            {
                var result = await _jobOfferRepository.GetLastPublishedDateByCompaniesIds(request.CompaniesIds);
                return Result<IReadOnlyList<KeyValueDateTimeDto>>.Success(result);
            }
        }
    }
}
