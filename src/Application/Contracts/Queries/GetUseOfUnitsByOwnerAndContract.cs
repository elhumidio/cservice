using Application.Contracts.DTO;
using Application.Core;
using Application.JobOffer.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Queries
{
    public class GetUseOfUnitsByOwnerAndContract
    {
        public class Query : IRequest<Result<AssignationsDto>>
        {
            public int ContractId { get; set; }
            public int OwnerId { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<AssignationsDto>>
        {
            private readonly IUnitsRepository _unitsRepo;
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;

            public Handler(IMapper mapper, IUnitsRepository unitsRepo,IMediator mediator)
            {
                _mapper = mapper;
                _unitsRepo = unitsRepo;
                _mediator = mediator;   
            }

            public async Task<Result<AssignationsDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                
                var assignments = _unitsRepo.GetAssignmentsByContractAndManager(request.ContractId, request.OwnerId)
                  .ProjectTo<UnitsAssignmentDto>(_mapper.ConfigurationProvider).AsQueryable();

                var unitsConsumed = await _mediator.Send(new ListActivesByManager.Query
                {
                    ContractID = request.ContractId,
                    OwnerID = request.OwnerId
                });

                var unitsAssignment = await _mediator.Send(new GetAssignedUnitsByOwner.Query
                { 
                    ContractId = request.ContractId,
                    OwnerId = request.OwnerId
                });

                AssignationsDto dto = new AssignationsDto
                {
                    ContractId = request.ContractId,
                    UnitsConsumed = unitsConsumed.Value,
                    UnitsAssigned = unitsAssignment.Value
                };

                return Result<AssignationsDto>.Success(dto);
            }
        }
    }
}
