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
    public class ListDegrees
    {

        public class Query : IRequest<Result<List<DegreeDTO>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<DegreeDTO>>>
        {
            private readonly IDegreeRepository _degree;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, IDegreeRepository degree)
            {
                _mapper = mapper;
                _degree = degree;
            }

            public async Task<Result<List<DegreeDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _degree.GetDegrees(request.siteID, request.languageID).ProjectTo<DegreeDTO>(_mapper.ConfigurationProvider);
                return Result<List<DegreeDTO>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
