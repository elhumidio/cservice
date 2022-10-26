using Application.Core;
using Application.JobOffer.Queries;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class GetAvailableUnitsMexicoOrPortugal
    {
        public class Query : IRequest<Result<int>>
        {
            public int CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
            private readonly IMediator _mediator;
            private readonly IUnitsRepository _unitsRepo;

            public Handler(
                IMediator mediator,
                IUnitsRepository unitsRepo)
            {
                _mediator = mediator;
                _unitsRepo = unitsRepo;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                var consumedUnits = _mediator.Send(new GetConsumedUnitsWelcomeNotSpain.Query
                {
                    CompanyId = request.CompanyId
                }).Result.Value.Count();
                var assignedUnits = _unitsRepo.GetAssignedUnitsMxPtByCompany(request.CompanyId);
                var unitsAvailable = assignedUnits - consumedUnits;
                return Result<int>.Success(unitsAvailable);
            }
        }
    }
}
