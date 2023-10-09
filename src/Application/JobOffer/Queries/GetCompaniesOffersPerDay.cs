using Application.Core;
using AutoMapper;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class GetCompaniesOffersPerDay
    {
        public class Get : IRequest<Result<List<CompanyOffersPerDayDto>>>
        {
            public DateTime SinceDate { get; set; }

        }

        public class Handler : IRequestHandler<Get, Result<List<CompanyOffersPerDayDto>>>
        {
            private readonly IJobOfferRepository _jobOfferRepository;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOffer)
            {
                _mapper = mapper;
                _jobOfferRepository = jobOffer;
            }

            public async Task<Result<List<CompanyOffersPerDayDto>>> Handle(Get request, CancellationToken cancellationToken)
            {
                return Result<List<CompanyOffersPerDayDto>>.Success(await _jobOfferRepository.GetCompaniesOffersPerDay(request.SinceDate));
            }
        }
    }
}
