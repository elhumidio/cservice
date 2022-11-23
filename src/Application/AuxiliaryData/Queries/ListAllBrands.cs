using Application.AuxiliaryData.DTO;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AuxiliaryData.Queries
{
    public class ListAllBrands
    {
        public class Query : IRequest<Result<List<BrandDTO>>>
        {
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
                var brands = await _brand.GetAllBrands();
                List<BrandDTO> result = new List<BrandDTO>();
                var a = _mapper.Map(brands, result);
                return Result<List<BrandDTO>>.Success(a);
            }
        }
    }
}
