using Application.AuxiliaryData.DTO;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AuxiliaryData.Queries
{
    public class ListBrands
    {
        public class Query : IRequest<Result<List<BrandDTO>>>
        {
            public int companyID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<BrandDTO>>>
        {
            private readonly IBrandRepository _brand;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IBrandRepository brand)
            {
                _mapper = mapper;
                _brand = brand;
            }

            public async Task<Result<List<BrandDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _brand.GetListBrands(request.companyID).ProjectTo<BrandDTO>(_mapper.ConfigurationProvider);
                return Result<List<BrandDTO>>.Success(await query.ToListAsync());
            }
        }
    }
}
