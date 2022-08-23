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
    public class ListJobContractTypes
    {
        public class Query : IRequest<Result<List<JobContractTypeDTO>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<JobContractTypeDTO>>>
        {
            private readonly IJobContractTypeRepository _jobContractType;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, IJobContractTypeRepository jobContractType)
            {
                _mapper = mapper;
                _jobContractType = jobContractType;
            }

            public async Task<Result<List<JobContractTypeDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _jobContractType.GetJobContractTypes(request.siteID, request.languageID).ProjectTo<JobContractTypeDTO>(_mapper.ConfigurationProvider);
                return Result<List<JobContractTypeDTO>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
