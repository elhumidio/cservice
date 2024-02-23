using Application.Core;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.JobOffer.Queries.ToggleOfferStatus;

namespace Application.JobOffer.Queries
{
    public class SetPlayPauseOfferStatus
    {
        public class Query : IRequest<Result<bool>>
        {
            public int State { get; set; }
            public int OfferId { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<bool>>
        {
            private readonly IMapper _mapper;
            private readonly IJobOfferRepository _jobOffer;

            public Handler(IMapper mapper, IJobOfferRepository jobOffer)
            {
                _mapper = mapper;
                _jobOffer = jobOffer;
            }

            public async Task<Result<bool>> Handle(Query request, CancellationToken cancellationToken)
            {
                var action = _jobOffer.SetPlayPauseOfferStatus(request.OfferId, request.State);
                return Result<bool>.Success(action);
            }
        }
    }
}
