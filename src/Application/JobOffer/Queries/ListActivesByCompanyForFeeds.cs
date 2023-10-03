using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffer.Queries
{
    public class ListActivesByCompanyForFeeds
    {

        public class Query : IRequest<Result<List<JobOfferResponse>>>
        {
            public int CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<JobOfferResponse>>>
        {
            private readonly IJobOfferRepository _jobOffer;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOffer)
            {
                _mapper = mapper;
                _jobOffer = jobOffer;
            }

            public async Task<Result<List<JobOfferResponse>>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<JobOfferResponse> query = null;
                query = _jobOffer.GetActiveOffersByCompany(request.CompanyId).ProjectTo<JobOfferResponse>(_mapper.ConfigurationProvider).AsQueryable();
                return Result<List<JobOfferResponse>>.Success(await query.ToListAsync());
            }
        }
    }
}
