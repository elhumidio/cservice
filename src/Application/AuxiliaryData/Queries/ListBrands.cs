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
    public class ListBrands
    {

        public class Query : IRequest<Result<List<BrandDto>>>
        {
            public int companyID { get; set; }
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
                var query = _brand.GetListBrands(request.companyID).ProjectTo<BrandDto>(_mapper.ConfigurationProvider);
                return Result<List<BrandDto>>.Success(await query.ToListAsync());                
            }         
        }
    }
}
