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
    public class ListRegions
    {

        public class Query : IRequest<Result<List<RegionDTO>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<RegionDTO>>>
        {
            private readonly IRegionRepository _region;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, IRegionRepository region)
            {
                _mapper = mapper;
                _region = region;
            }

            public async Task<Result<List<RegionDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _region.GetRegions(request.siteID, request.languageID).ProjectTo<RegionDTO>(_mapper.ConfigurationProvider);
                return Result<List<RegionDTO>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
