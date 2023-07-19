using Application.Core;
using AutoMapper;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class GetOffersForView
    {
        public class Get : IRequest<Result<List<OfferInfoMin>>>
        {
            public int[] OfferIds { get; set; }
            public int Language { get; set; }
           
        }

        public class Handler : IRequestHandler<Get, Result<List<OfferInfoMin>>>
        {
            private readonly IJobOfferRepository _jobOfferRepository;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOffer)
            {
                _mapper = mapper;
                _jobOfferRepository = jobOffer;
            }

            public async Task<Result<List<OfferInfoMin>>> Handle(Get request, CancellationToken cancellationToken)
            {                
                List<OfferInfoMin> query = await _jobOfferRepository.GetOffersForView(request.OfferIds, request.Language);
                return Result<List<OfferInfoMin>>.Success(query);
            }
        }
    }
}
