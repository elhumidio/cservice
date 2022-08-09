using Application.Contracts.DTO;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Contracts.Queries
{
    public class GetAssignments
    {

        public class Query : IRequest<Result<List<UnitsAssignmentDTO>>>
        {
            public int ContractID { get; set; }
            public int OwnerID { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UnitsAssignmentDTO>>>
        {
            private readonly IUnitsRepository _unitsRepo;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, IUnitsRepository unitsRepo)
            {
                _mapper = mapper;
                _unitsRepo = unitsRepo;
            }

            public async Task<Result<List<UnitsAssignmentDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var assignments = _unitsRepo.GetAssignmentsByContractAndManager(request.ContractID, request.OwnerID)
                    .ProjectTo<UnitsAssignmentDTO>(_mapper.ConfigurationProvider)
                    .AsQueryable();
                return Result<List<UnitsAssignmentDTO>>.Success(await assignments.ToListAsync());
            }


        }
    }
}
