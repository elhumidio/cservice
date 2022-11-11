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
        public class Query : IRequest<Result<List<AssignationsDto>>>
        {
            public List<int> ContractIds { get; set; }
            public List<int> OwnerIds { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<List<AssignationsDto>>>
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

            public async Task<Result<List<AssignationsDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<AssignationsDto> assignationList = new();

                foreach (var contract in request.ContractIds)
                {
                    foreach (var owner in request.OwnerIds)
                    {
                        var unitsAssignment = await _mediator.Send(new GetAssignedUnitsByOwner.Query
                        {
                            ContractId = contract,
                            OwnerId = owner
                        });

                        var unitsConsumed = await _mediator.Send(new ListActivesByManager.Query
                        {
                            ContractID = contract,
                            OwnerID = owner
                        });
                        AssignationsDto dto = new AssignationsDto
                        {
                            OwnerId = owner,    
                            ContractId = contract,  
                            UnitsConsumed = unitsConsumed.Value,
                            UnitsAssigned = unitsAssignment.Value
                        };
                        assignationList.Add(dto);

                    }
                }

                return Result<List<AssignationsDto>>.Success(assignationList);
            }


            
        }
    }
}
