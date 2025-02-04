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
                try {
                    var titles = await _title.GetByLanguage(request.LangId);
                    return Result<List<TitleLang>>.Success(titles);
                }
                catch(Exception ex) {
                    var a = ex;
                    return null;
                }
                
            }
        }
    }
}
