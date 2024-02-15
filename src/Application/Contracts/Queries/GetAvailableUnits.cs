using Application.Contracts.DTO;
using Application.Core;
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

            public Handler(IJobOfferRepository jobOfferRepo, IContractProductRepository contractProductRepo, IUnitsRepository unitsRepo)
            {
                _jobOfferRepo = jobOfferRepo;
                _contractProductRepo = contractProductRepo;
                _unitsRepo = unitsRepo;
            }

            public async Task<Result<List<AvailableUnitsDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return GetAvailableUnits(request.ContractId).Result;
            }

            public async Task<Result<List<AvailableUnitsDto>>> GetAvailableUnits(int contractId, VacancyType vacancyType = VacancyType.None)
            {
                var list = new List<AvailableUnitsDto>();
                AvailableUnitsDto dto;
                var isPack = _contractProductRepo.IsPack(contractId);
                var unitsAssignedToUsers = _unitsRepo.GetAssignmentsByContract(contractId).ToList();

                foreach (var units in unitsAssignedToUsers)
                {
                    var vacancyTypetoUse = vacancyType == VacancyType.None ? units.IdjobVacType : (int)vacancyType;

                    var unitsConsumed = _jobOfferRepo.GetActiveOffersByContractOwnerType(contractId, units.IdenterpriseUser, vacancyTypetoUse).Count();
                    dto = new AvailableUnitsDto
                    {
                        ContractId = contractId,
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
