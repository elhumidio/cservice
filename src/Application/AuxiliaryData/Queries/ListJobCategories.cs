using Application.AuxiliaryData.DTO;
using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AuxiliaryData.Queries
{
    public class ListJobCategories
    {

        public class Query : IRequest<Result<List<JobCategoryDTO>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<JobCategoryDTO>>>
        {
            private readonly IJobCategoryRepository _jobCategory;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, IJobCategoryRepository jobCategory)
            {
                _mapper = mapper;
                _jobCategory = jobCategory;
            }

            public async Task<Result<List<JobCategoryDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _jobCategory.GetJobCategories(request.siteID, request.languageID).ProjectTo<JobCategoryDTO>(_mapper.ConfigurationProvider);
                return Result<List<JobCategoryDTO>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
