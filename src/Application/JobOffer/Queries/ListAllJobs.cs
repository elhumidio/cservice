using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Classes;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffer.Queries
{
    public class ListAllJobs
    {
        public class Query : IRequest<Result<List<JobData>>>
        {
            public int LanguageId { get; set; }
            public int SiteId { get; set; }
            public int CountryId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<JobData>>>
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

            public async Task<Result<List<JobData>>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<JobData> query = null;
                query = await _jobOffer.GetAllJobs();
                return Result<List<JobData>>.Success(await query.ToListAsync());
            }
        }
    }
}
