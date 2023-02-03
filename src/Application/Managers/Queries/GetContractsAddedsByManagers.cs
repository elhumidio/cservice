using Application.Contracts.DTO;
using Application.Core;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Managers.Queries
{
    public class GetContractsAddedsByManagers
    {
        public class Get : IRequest<Result<Dictionary<int,List<int>>>>
        {
            public List<int> Managers { get; set; }
        }

        public class Handler : IRequestHandler<Get, Result<Dictionary<int, List<int>>>>
        {
            private readonly IUnitsRepository _unitsRepository;
            

            public Handler(IMapper mapper, IUnitsRepository unitsRepository)
            {
            
                _unitsRepository = unitsRepository; 
            }

            public async Task<Result<Dictionary<int, List<int>>>> Handle(Get request, CancellationToken cancellationToken)
            {
                var query = await _unitsRepository.GetAssignedContractsForManagers(request.Managers);
                return Result<Dictionary<int, List<int>>>.Success(query);
            }
        }
    }
}
