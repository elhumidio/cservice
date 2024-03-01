using Application.Contracts.DTO;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Contracts.Queries
{
    public class List
    {
        public class Query : IRequest<Result<List<ContractDto>>>
        {
            public int CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<ContractDto>>>
        {
            private readonly IContractRepository _contractRepo;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IContractRepository contractRepo)
            {
                _mapper = mapper;
                _contractRepo = contractRepo;
            }

            public async Task<Result<List<ContractDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _contractRepo.GetContracts(request.CompanyId).ProjectTo<ContractDto>(_mapper.ConfigurationProvider).AsQueryable();
                return Result<List<ContractDto>>.Success(await query.ToListAsync());
            }
        }
    }


    public class ListPayments
    {
        public class Query : IRequest<Result<ContractPayment>>
        {
            public int CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ContractPayment>>
        {
            private readonly IContractRepository _contractRepo;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IContractRepository contractRepo)
            {
                _mapper = mapper;
                _contractRepo = contractRepo;
            }

            public async Task<Result<ContractPayment>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _contractRepo.GetContracts(request.CompanyId).ProjectTo<ContractDto>(_mapper.ConfigurationProvider).AsQueryable();
                return Result<ContractPayment>.Success(await query.ToListAsync());
            }
        }
    }
}
