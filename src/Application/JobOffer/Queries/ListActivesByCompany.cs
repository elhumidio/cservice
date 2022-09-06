using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffer.Queries
{
    public class ListActivesByCompany
    {
        public class Query : IRequest<Result<List<JobOfferDto>>>
        {
            public int CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<JobOfferDto>>>
        {
            private readonly IJobOfferRepository _jobOffer;
            private readonly IContractProductRepository _contractProductRepo;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOffer, IContractProductRepository contractProductRepo)
            {
                _mapper = mapper;
                _jobOffer = jobOffer;
                _contractProductRepo = contractProductRepo;
            }

            public async Task<Result<List<JobOfferDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<JobOfferDto> query = null;
                query = _jobOffer.GetActiveOffersByCompany(request.CompanyId).ProjectTo<JobOfferDto>(_mapper.ConfigurationProvider).AsQueryable();
                return Result<List<JobOfferDto>>.Success(await query.ToListAsync());
            }
        }
    }
}
