using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Classes;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffer.Queries
{
    public class GetActiveOffers
    {
        public class Query : IRequest<Result<List<JobOfferDto>>>
        {
            
        }

        public class Handler : IRequestHandler<Query, Result<List<JobOfferDto>>>
        {
            private readonly IJobOfferRepository _jobOfferRepository;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOffer)
            {
                _mapper = mapper;
                _jobOfferRepository = jobOffer;
            }

            public async Task<Result<List<JobOfferDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<JobOfferDto> query = _jobOfferRepository.GetActiveOffers().ProjectTo<JobOfferDto>(_mapper.ConfigurationProvider).AsQueryable();
                return Result<List<JobOfferDto>>.Success(await query.ToListAsync());
            }
        }

    }


}
