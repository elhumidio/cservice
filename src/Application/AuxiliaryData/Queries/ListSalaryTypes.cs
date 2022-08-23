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
    public class ListSalaryTypes
    {

        public class Query : IRequest<Result<List<SalaryTypeDto>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<SalaryTypeDto>>>
        {
            private readonly ISalaryTypeRepository _salaryType;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, ISalaryTypeRepository salaryType)
            {
                _mapper = mapper;
                _salaryType = salaryType;
            }

            public async Task<Result<List<SalaryTypeDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _salaryType.GetSalaryTypes(request.siteID, request.languageID).ProjectTo<SalaryTypeDto>(_mapper.ConfigurationProvider);
                return Result<List<SalaryTypeDto>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
