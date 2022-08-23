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
    public class ListCountries
    {

        public class Query : IRequest<Result<List<CountryDTO>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<CountryDTO>>>
        {
            private readonly ICountryRepository _country;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, ICountryRepository country)
            {
                _mapper = mapper;
                _country = country;
            }

            public async Task<Result<List<CountryDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _country.GetCountries(request.siteID, request.languageID).ProjectTo<CountryDTO>(_mapper.ConfigurationProvider);
                return Result<List<CountryDTO>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
