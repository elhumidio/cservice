using Application.Contracts.DTO;
using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Persistence.Enums;

namespace Application.Contracts.Queries
{
    public class GetAvailableUnitsByOwner
    {

        public class Query : IRequest<Result<List<AvailableUnitsDTO>>>
        {
            public int ContractId { get; set; }
            public int OwnerId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<AvailableUnitsDTO>>>
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

            public async Task<Result<List<AvailableUnitsDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var list = new List<AvailableUnitsDTO>();
                AvailableUnitsDTO dto;
                var isPack = _contractProductRepo.IsPack(request.ContractId);
                var unitsAssigned = _unitsRepo.GetAssignmentsByContractAndManager(request.ContractId, request.OwnerId).ToList();

                var activeOffers = isPack ? _jobOfferRepo.GetActiveOffersByContractAndManager(request.ContractId, request.OwnerId)
                .ProjectTo<JobOfferDTO>(_mapper.ConfigurationProvider)
                .ToList().GroupBy(g => g.IdjobVacType)
                :
                _jobOfferRepo.GetActiveOffersByContractAndManagerNoPack(request.ContractId, request.OwnerId)
                .ProjectTo<JobOfferDTO>(_mapper.ConfigurationProvider)
                .ToList()
                .GroupBy(g => g.IdjobVacType);


                foreach (var units in unitsAssigned)
                {
                    var unitsConsumed = isPack ? _jobOfferRepo.GetActiveOffersByContractOwnerType(request.ContractId, units.IdenterpriseUser, units.IdjobVacType).Count()
                       : _jobOfferRepo.GetActiveOffersByContractOwnerTypeNoPack(request.ContractId, units.IdenterpriseUser, units.IdjobVacType).Count();

                    dto = new AvailableUnitsDTO
                    {
                        ContractId = request.ContractId,
                        IsPack = isPack,
                        OwnerId = units.IdenterpriseUser,
                        type = Enum.IsDefined(typeof(StandardWiseVacancyType), (StandardWiseVacancyType)units.IdjobVacType) ? 0 : (VacancyType)units.IdjobVacType,
                        Units = units.JobVacUsed - unitsConsumed
                    };
                    list.Add(dto);
                }

                return Result<List<AvailableUnitsDTO>>.Success(await Task.FromResult(list));
            }
        }
    }
}
