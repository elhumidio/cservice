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
    public class ListSites
    {

        public class Query : IRequest<Result<List<SiteDTO>>>
        {

        }

        public class Handler : IRequestHandler<Query, Result<List<SiteDTO>>>
        {
            private readonly ISiteRepository _site;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, ISiteRepository site)
            {
                _mapper = mapper;
                _site = site;
            }

            public async Task<Result<List<SiteDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _site.GetSites().ProjectTo<SiteDTO>(_mapper.ConfigurationProvider);
                return Result<List<SiteDTO>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
