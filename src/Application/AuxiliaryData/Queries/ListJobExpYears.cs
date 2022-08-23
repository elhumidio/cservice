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
    public class ListJobExpYears
    {

        public class Query : IRequest<Result<List<JobExpYearDto>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<JobExpYearDto>>>
        {
            private readonly IJobExpYearsRepository _jobExpYears;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, IJobExpYearsRepository jobExpYears)
            {
                _mapper = mapper;
                _jobExpYears = jobExpYears;
            }

            public async Task<Result<List<JobExpYearDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _jobExpYears.GetJobExperienceYears(request.siteID, request.languageID).ProjectTo<JobExpYearDto>(_mapper.ConfigurationProvider);
                return Result<List<JobExpYearDto>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
