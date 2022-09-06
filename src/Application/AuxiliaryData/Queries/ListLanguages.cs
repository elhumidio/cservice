using Application.AuxiliaryData.DTO;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AuxiliaryData.Queries
{
    public class ListLanguages
    {
        public class Query : IRequest<Result<List<LanguageDTO>>>
        {
            public int siteID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<LanguageDTO>>>
        {
            private readonly ILanguageRepository _language;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, ILanguageRepository language)
            {
                _mapper = mapper;
                _language = language;
            }

            public async Task<Result<List<LanguageDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _language.GetLanguages(request.siteID).ProjectTo<LanguageDTO>(_mapper.ConfigurationProvider);
                return Result<List<LanguageDTO>>.Success(await query.ToListAsync());
            }
        }
    }
}
