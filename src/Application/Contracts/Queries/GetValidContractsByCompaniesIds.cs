using Application.Core;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class GetValidContractsByCompaniesIds
    {
        public class Get : IRequest<Result<IReadOnlyList<KeyValueResponse>>>
        {
            public List<int> CompaniesIds { get; set; }
        }

        public class Handler : IRequestHandler<Get, Result<IReadOnlyList<KeyValueResponse>>>
        {
            private readonly IContractRepository _contractRepository;

            public Handler(IContractRepository contractRepository)
            {
                _contractRepository = contractRepository;
            }

            public async Task<Result<IReadOnlyList<KeyValueResponse>>> Handle(Get request, CancellationToken cancellationToken)
            {
                var result = await _contractRepository.GetValidContractsByCompaniesIds(request.CompaniesIds);
                return Result<IReadOnlyList<KeyValueResponse>>.Success(result);
            }
        }
    }


    public class GetValidContractsAndpoductsByCompanyIds
    {
        public class Get : IRequest<Result<IReadOnlyList<KeyValueResponse>>>
        {
            public List<int> CompaniesIds { get; set; }
        }

        public class Handler : IRequestHandler<Get, Result<IReadOnlyList<KeyValueResponse>>>
        {
            private readonly IContractRepository _contractRepository;

            public Handler(IContractRepository contractRepository)
            {
                _contractRepository = contractRepository;
            }

            public async Task<Result<IReadOnlyList<KeyValueResponse>>> Handle(Get request, CancellationToken cancellationToken)
            {
                var result = await _contractRepository.GetValidContractsByCompaniesIds(request.CompaniesIds);
                return Result<IReadOnlyList<KeyValueResponse>>.Success(result);
            }
        }
    }
}
