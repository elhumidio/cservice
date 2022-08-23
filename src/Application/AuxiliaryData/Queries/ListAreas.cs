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
    public class ListAreas
    {

        public class Query : IRequest<Result<List<AreaDTO>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<AreaDTO>>>
        {
            private readonly IAreaRepository _area;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, IAreaRepository area)
            {
                _mapper = mapper;
                _area = area;
            }

            public async Task<Result<List<AreaDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query =  _area.GetAreas(request.siteID, request.languageID).ProjectTo<AreaDTO>(_mapper.ConfigurationProvider);
                return Result<List<AreaDTO>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
