using Application.AuxiliaryData.DTO;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AuxiliaryData.Queries
{
    public class ListJobExpYears
    {
        public class Query : IRequest<Result<List<JobExpYearDTO>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<JobExpYearDTO>>>
        {
            private readonly IJobExpYearsRepository _jobExpYears;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobExpYearsRepository jobExpYears)
            {
                _mapper = mapper;
                _jobExpYears = jobExpYears;
            }

            public async Task<Result<List<JobExpYearDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _jobExpYears.GetJobExperienceYears(request.siteID, request.languageID).ProjectTo<JobExpYearDTO>(_mapper.ConfigurationProvider);
                return Result<List<JobExpYearDTO>>.Success(await query.ToListAsync());
            }
        }
    }
}
