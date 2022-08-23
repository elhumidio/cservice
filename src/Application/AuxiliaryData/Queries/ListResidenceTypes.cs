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
    public class ListResidenceTypes
    {

        public class Query : IRequest<Result<List<ResidenceTypeDto>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ResidenceTypeDto>>>
        {
            private readonly IResidenceTypeRepository _residenceType;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, IResidenceTypeRepository residenceType)
            {
                _mapper = mapper;
                _residenceType = residenceType;
            }

            public async Task<Result<List<ResidenceTypeDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _residenceType.GetResidenceTypes(request.siteID, request.languageID).ProjectTo<ResidenceTypeDto>(_mapper.ConfigurationProvider);
                return Result<List<ResidenceTypeDto>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
