using Application.AuxiliaryData.DTO;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AuxiliaryData.Queries
{
    public class ListSalaryTypes
    {
        public class Query : IRequest<Result<List<SalaryTypeDTO>>>
        {
            public int siteID { get; set; }
            public int languageID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<SalaryTypeDTO>>>
        {
            private readonly ISalaryTypeRepository _salaryType;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, ISalaryTypeRepository salaryType)
            {
                _mapper = mapper;
                _salaryType = salaryType;
            }

            public async Task<Result<List<SalaryTypeDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _salaryType.GetSalaryTypes(request.siteID, request.languageID).ProjectTo<SalaryTypeDTO>(_mapper.ConfigurationProvider);
                return Result<List<SalaryTypeDTO>>.Success(await query.ToListAsync());
            }
        }
    }
}
