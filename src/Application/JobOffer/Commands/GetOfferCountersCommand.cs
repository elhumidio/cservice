using Application.Core;
using Domain.DTO.ManageJobs;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Commands
{
    public class GetOfferCountersCommand
    {
        public class Handler : IRequestHandler<GetCountersQuery, Result<CounterOffersByState>>
        {
            private readonly IJobOfferRepository _jobOfferRepository;

            public Handler(
                IJobOfferRepository jobOfferRepository
                )
            {
                _jobOfferRepository = jobOfferRepository;
            }

            public async Task<Result<CounterOffersByState>> Handle(GetCountersQuery request, CancellationToken cancellationToken)
            {
                var counters = _jobOfferRepository.GetOffersCounters(request.CompanyId);
                
                return  Result<CounterOffersByState>.Success(new CounterOffersByState { Counts = counters});

            }
        }
    }
}
