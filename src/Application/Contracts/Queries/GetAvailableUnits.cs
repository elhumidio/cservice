using Application.Contracts.DTO;
using Application.Core;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class GetAvailableUnits
    {
        public class Query : IRequest<Result<List<AvailableUnitsDto>>>
        {
            public int ContractId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<AvailableUnitsDto>>>
        {
            private readonly IJobOfferRepository _jobOfferRepo;
            private readonly IContractProductRepository _contractProductRepo;
            private readonly IUnitsRepository _unitsRepo;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOfferRepo, IContractProductRepository contractProductRepo, IUnitsRepository unitsRepo)
            {
                _mapper = mapper;
                _jobOfferRepo = jobOfferRepo;
                _contractProductRepo = contractProductRepo;
                _unitsRepo = unitsRepo;
            }

            public async Task<Result<List<AvailableUnitsDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var list = new List<AvailableUnitsDto>();
                AvailableUnitsDto dto;
                var isPack = _contractProductRepo.IsPack(request.ContractId);
                var unitsAssigned = _unitsRepo.GetAssignmentsByContract(request.ContractId).ToList();

                foreach (var units in unitsAssigned)
                {
                    var unitsConsumed = isPack ? _jobOfferRepo.GetActiveOffersByContractOwnerType(request.ContractId, units.IdenterpriseUser, units.IdjobVacType).Count()
                         : _jobOfferRepo.GetActiveOffersByContractAndTypeNoPack(request.ContractId, units.IdjobVacType).Count();
                    dto = new AvailableUnitsDto
                    {
                        ContractId = request.ContractId,
                        IsPack = isPack,
                        OwnerId = units.IdenterpriseUser,
                        type = (VacancyType)units.IdjobVacType,
                        Units = units.JobVacUsed - unitsConsumed
                    };
                    list.Add(dto);
                }
                var orderedList = list.OrderBy(o => o.type).ToList();
                return Result<List<AvailableUnitsDto>>.Success(await Task.FromResult(orderedList));
            }
        }
    }
}
