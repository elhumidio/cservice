using Application.AuxiliaryData.DTO;
using Application.Core;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AuxiliaryData.Queries
{
    public class ListTitles
    {
        public class Query : IRequest<Result<List<TitleLang>>>
        {
            public int LangId { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<List<TitleLang>>>
        {
            private readonly ITitleRepository _title;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, ITitleRepository title)
            {
                _mapper = mapper;
                _title = title;
            }

            public async Task<Result<List<TitleLang>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var brands = await _title.GetByLanguage(request.LangId);
                List<BrandDTO> result = new List<BrandDTO>();
                var a = _mapper.Map(brands, result);
                return Result<List<BrandDTO>>.Success(a);
            }
        }
    }
}
