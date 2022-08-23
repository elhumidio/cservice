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
    public class ListSalaries
    {

        public class Query : IRequest<Result<List<SalaryDto>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<SalaryDto>>>
        {
            private readonly ISalaryRepository _salary;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, ISalaryRepository salary)
            {
                _mapper = mapper;
                _salary = salary;
            }

            public async Task<Result<List<SalaryDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _salary.GetSalaries(request.siteID, request.languageID).ProjectTo<SalaryDto>(_mapper.ConfigurationProvider);
                return Result<List<SalaryDto>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
