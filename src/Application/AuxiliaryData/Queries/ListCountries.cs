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

        public class Query : IRequest<Result<List<CountryDto>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<CountryDto>>>
        {
            private readonly ICountryRepository _country;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, ICountryRepository country)
            {
                _mapper = mapper;
                _country = country;
            }

            public async Task<Result<List<CountryDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _country.GetCountries(request.siteID, request.languageID).ProjectTo<CountryDto>(_mapper.ConfigurationProvider);
                return Result<List<CountryDto>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
