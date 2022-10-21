using Application.Contracts.DTO;
using Application.Core;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class GetUnitsByCompany
    {
        public class Query : IRequest<Result<List<UnitsDto>>>
        {
            public int CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UnitsDto>>>
        {
            private readonly IContractRepository _contractRepo;
            private readonly IRegEnterpriseContractRepository _regEnterpriseContractRepo;
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;

            public Handler(IMapper mapper, IContractRepository contractRepo, IMediator mediator, IRegEnterpriseContractRepository regEnterpriseContractRepo)
            {
                _mapper = mapper;
                _contractRepo = contractRepo;
                _mediator = mediator;
                _regEnterpriseContractRepo = regEnterpriseContractRepo;
            }

            public async Task<Result<List<UnitsDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<UnitsDto> list = new List<UnitsDto>();
                var contracts = _contractRepo.GetContracts(request.CompanyId).ToList();
                foreach (var contract in contracts)
                {
                    var units = _mediator.Send(new GetAvailableUnits.Query
                    {
                        ContractId = contract.Idcontract
                    }).Result.Value.GroupBy(u => u.type);

                    foreach (var unitType in units)
                    {
                        UnitsDto dto = new();
                        dto.IDContract = contract.Idcontract;
                        dto.AvailableUnits = unitType.Sum(ut => ut.Units);
                        dto.JobVacTypeId = (int)unitType.First().type;
                        dto.JobVacTypeDesc = Enum.GetName(typeof(VacancyType), unitType.First().type);
                        dto.TotalUnits = await _regEnterpriseContractRepo.GetUnitsByType(contract.Idcontract, unitType.First().type);
                        list.Add(dto);
                    }
                }

                return Result<List<UnitsDto>>.Success(list);
            }
        }
    }
}
