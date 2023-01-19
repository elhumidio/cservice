using Application.Core;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.EnterpriseContract.Queries
{
    public class CountCompaniesActive
    { 
        public class Query : IRequest<Result<int>>
        {

        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
            private readonly IEnterpriseRepository _enterpriseRepo;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IEnterpriseRepository enterpriseRepo)
            {
                _mapper = mapper;
                _enterpriseRepo = enterpriseRepo;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<int>.Success(await _enterpriseRepo.GetCountCompaniesActive());
            }
        }
    }
}
