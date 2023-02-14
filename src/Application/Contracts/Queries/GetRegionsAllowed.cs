using Application.Contracts.DTO;
using Application.Core;
using AutoMapper;
using Domain.DTO;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Queries
{
    public class GetRegionsAllowed
    {
        public class GetRegions : IRequest<Result<List<RegionsAllowedDto>>>
        {
            public int ContractId { get; set; }
            
        }

        public class Handler : IRequestHandler<GetRegions, Result<List<RegionsAllowedDto>>>
        {
            private readonly IContractPublicationRegionRepository _regionsAllowedRepo;


            public Handler(IContractPublicationRegionRepository regionsAllowedRepo)
            {
                _regionsAllowedRepo= regionsAllowedRepo;    
            }

            public async Task<Result<List<RegionsAllowedDto>>> Handle(GetRegions request, CancellationToken cancellationToken)
            {
                var products = await _regionsAllowedRepo.GetAllowedRegionsNamesByContract(request.ContractId);
                return Result<List<RegionsAllowedDto>>.Success(products);
            }
        }
    }
}
