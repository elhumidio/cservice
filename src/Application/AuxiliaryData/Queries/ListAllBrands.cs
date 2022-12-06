using Application.AuxiliaryData.DTO;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.DTO;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.AuxiliaryData.Queries
{
    public class ListAllBrands
    {
        public class Query : IRequest<Result<List<BrandDto>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<BrandDto>>>
        {
            private readonly IBrandRepository _brand;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IBrandRepository brand)
            {
                _mapper = mapper;
                _brand = brand;
            }

            public async Task<Result<List<BrandDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var brands = await _brand.GetAllBrands();
                List<BrandDTO> result = new List<BrandDTO>();                
                return Result<List<BrandDto>>.Success(brands);
            }
        }
    }
}
