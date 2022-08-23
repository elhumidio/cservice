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
    public class ListJobVacTypes
    {

        public class Query : IRequest<Result<List<JobVacTypeDTO>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<JobVacTypeDTO>>>
        {
            private readonly IJobVacTypeRepository _jobVacType;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, IJobVacTypeRepository jobVacType)
            {
                _mapper = mapper;
                _jobVacType = jobVacType;
            }

            public async Task<Result<List<JobVacTypeDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _jobVacType.GetJobVacTypes(request.siteID, request.languageID).ProjectTo<JobVacTypeDTO>(_mapper.ConfigurationProvider);
                return Result<List<JobVacTypeDTO>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
