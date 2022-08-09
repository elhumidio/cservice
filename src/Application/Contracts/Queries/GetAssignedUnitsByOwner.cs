using Application.Contracts.DTO;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Contracts.Queries
{
    public class GetAssignedUnitsByOwner
    {
        public class Query : IRequest<Result<List<UnitsAssignmentDTO>>>
        {
            public int ContractId { get; set; }
            public int OwnerId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UnitsAssignmentDTO>>>
        {
            private readonly IUnitsRepository _unitsRepo;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOfferRepo, IContractProductRepository contractProductRepo, IUnitsRepository unitsRepo)
            {
                _mapper = mapper;
                _unitsRepo = unitsRepo;
            }

            public async Task<Result<List<UnitsAssignmentDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _unitsRepo.GetAssignmentsByContractAndManager(request.ContractId, request.OwnerId).ProjectTo<UnitsAssignmentDTO>(_mapper.ConfigurationProvider).AsQueryable();
                return Result<List<UnitsAssignmentDTO>>.Success(
                    await query.ToListAsync()
                );
            }
        }
    }
}
