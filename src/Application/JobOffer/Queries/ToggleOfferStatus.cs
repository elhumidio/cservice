using Application.Core;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class ToggleOfferStatus
    {
        public class Toggle : IRequest<Result<bool>>
        {
            public int OfferId { get; set; }
        }

        public class Handler : IRequestHandler<Toggle, Result<bool>>
        {
            private readonly IJobOfferRepository _jobOffer;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOffer)
            {
                _mapper = mapper;
                _jobOffer = jobOffer;
            }

            public async Task<Result<bool>> Handle(Toggle request, CancellationToken cancellationToken)
            {
                var action = _jobOffer.ToggleOfferStatus(request.OfferId);
                return Result<bool>.Success(action);
            }
        }
    }
}
