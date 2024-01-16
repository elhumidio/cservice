using Application.Core;
using Domain.Enums;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class GetContractForEnterprise
    {
        public class Query : IRequest<Result<int>>
        {
            public VacancyType VacancyType { get; set; }
            public int EnterpriseId { get; set; }
            public int EnterpriseUserId { get; set; }

            public Query(VacancyType vacancyType, int enterpriseId, int enterpriseUserId)
            {
                VacancyType = vacancyType;
                EnterpriseId = enterpriseId;
                EnterpriseUserId = enterpriseUserId;
            }
        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
            private readonly IContractRepository _contractRepository;
            private readonly IJobOfferRepository _jobOfferRepository;
            private readonly IContractProductRepository _contractProductRepository;
            private readonly IUnitsRepository _unitsRepository;

            public Handler(IContractRepository contractRepository, IJobOfferRepository jobOfferRepository,
                IContractProductRepository contractProductRepository, IUnitsRepository unitsRepository)
            {
                _contractRepository = contractRepository;
                _jobOfferRepository = jobOfferRepository;
                _contractProductRepository = contractProductRepository;
                _unitsRepository = unitsRepository;
            }

            public Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                //For our contract type, find all the matching contracts that are in date
                var contracts = _contractRepository.GetContracts(request.EnterpriseId).OrderBy(contract => contract.FinishDate).ToList();

                var handler = new GetAvailableUnits.Handler(_jobOfferRepository, _contractProductRepository, _unitsRepository);

                //In order, the contract ending soonest is first.
                foreach (var contract in contracts)
                {
                    if (handler.GetAvailableUnits(contract.Idcontract, request.VacancyType).Result.Value
                        .FirstOrDefault(c => c.type == request.VacancyType && c.OwnerId == request.EnterpriseUserId)
                        ?.Units > 0)
                    {
                        return Task.FromResult(Result<int>.Success(contract.Idcontract));
                    }
                }
                return Task.FromResult(Result<int>.Failure($"No Valid contracts with units for enterprise {request.EnterpriseId} and Contract Type {request.VacancyType}"));

            }
        }
    }
}
