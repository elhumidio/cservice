using Application.Core;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class GetFinishDateContractClosingExpiringByCompaniesIds
    {
        public class Get : IRequest<Result<IReadOnlyList<KeyValueDateTimeDto>>>
        {
            public List<int> CompaniesIds { get; set; }
        }

        public class Handler : IRequestHandler<Get, Result<IReadOnlyList<KeyValueDateTimeDto>>>
        {
            private readonly IContractRepository _contractRepository;

            public Handler(IContractRepository contractRepository)
            {
                _contractRepository = contractRepository;
            }

            public async Task<Result<IReadOnlyList<KeyValueDateTimeDto>>> Handle(Get request, CancellationToken cancellationToken)
            {
                var result = await _contractRepository.GetFinishDateContractClosingExpiringByCompaniesIds(request.CompaniesIds);
                return Result<IReadOnlyList<KeyValueDateTimeDto>>.Success(result);
            }
        }
    }
}
