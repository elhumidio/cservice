using Domain.Repositories;
using MediatR;
using Persistence.Repositories;

namespace Application.Contracts.Queries
{
    public class GetAllActiveCompanies
    {
        public class GetAll : IRequest<List<int>>
        {
        }

        public class Handler : IRequestHandler<GetAll, List<int>>
        {
            private readonly IEnterpriseRepository _enterpriseRepository;

            public Handler(IEnterpriseRepository enterpriseRepository)
            {
                _enterpriseRepository = enterpriseRepository;
            }

            public Task<List<int>> Handle(GetAll request, CancellationToken cancellationToken)
            {
                var values = _enterpriseRepository.GetCompaniesWithActiveJobs();
                return Task.FromResult(values);
            }
        }

    }
}
