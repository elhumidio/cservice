using Application.Contracts.Queries;
using Application.Core;
using Application.EnterpriseContract.DTO;
using Domain.DTO;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.EnterpriseContract.Queries
{
    public class GetCompanyInfoManagers
    {
        public class Query : IRequest<Result<AtsmanagerAdminRegion>>
        {
            public GetCompanyRequest CompanyData { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<AtsmanagerAdminRegion>>
        {
            private const int _globalManager = 999;
            private readonly IEnterpriseUserRepository _enterpriseUserRepository;
            private readonly IUserRepository _userRepository;
            private readonly IATSManagerAdminRepository _atSManagerAdminRepository;
            private readonly IMediator _mediatr;

            public Handler(IEnterpriseUserRepository enterpriseUserRepo,
                IUserRepository userRepo,
                IATSManagerAdminRepository aTSManagerAdminRepository,
                IMediator mediator)
            {
                _enterpriseUserRepository = enterpriseUserRepo;
                _userRepository = userRepo;
                _atSManagerAdminRepository = aTSManagerAdminRepository;
                _mediatr = mediator;
            }

            public async Task<Result<AtsmanagerAdminRegion>> Handle(Query request, CancellationToken cancellationToken)
            {
                CompanyinfoDto obj = new()
                {
                    IDSUser = _userRepository.GetUserIdByEmail(request.CompanyData.Email)
                };

                var managers = _atSManagerAdminRepository.Get(request.CompanyData.CompanyId);
                var ableManagers = new List<AtsmanagerAdminRegion>();
                var winnerManager = new AtsmanagerAdminRegion();
                if (managers != null)
                {
                    var globalManager = _atSManagerAdminRepository.GetGlobalOwner(request.CompanyData.CompanyId);

                    var managersByRegion = managers.Where(r => r.RegionId == request.CompanyData.RegionId).ToList();

                    if (managersByRegion != null && managersByRegion.Any())
                    {
                        ableManagers.AddRange(managersByRegion);
                    }
                    else {
                        var managersByCountry = managers.Where(r => r.CountryId == request.CompanyData.CountryId).ToList();
                        ableManagers.AddRange(managersByCountry);
                    }
                    
                   
                    if (ableManagers.Any())
                    {
                        foreach (var manager in ableManagers)
                        {
                            var result = await _mediatr.Send(new GetAvailableUnitsByOwner.Query
                            {
                                ContractId = request.CompanyData.ContractId,
                                OwnerId = manager.ManagerId
                            });
                            var available = result.Value.FirstOrDefault(v => (int)v.type == request.CompanyData.IdJobVacType && v.Units > 0);
                            if (available != null)
                            {
                                winnerManager = manager;
                                return new Result<AtsmanagerAdminRegion> { Value = winnerManager };
                            }
                        }
                    }
                    else
                    {
                        if (globalManager.ManagerId > 0) {
                            var result = await _mediatr.Send(new GetAvailableUnitsByOwner.Query
                            {
                                ContractId = request.CompanyData.ContractId,
                                OwnerId = globalManager.ManagerId
                            });
                            var available = result.Value.FirstOrDefault(v => (int)v.type == request.CompanyData.IdJobVacType && v.Units > 0);
                            if (available != null) {
                                return new Result<AtsmanagerAdminRegion> { Value = globalManager };
                            }
                            else return new Result<AtsmanagerAdminRegion>();
                        }
                    }
                }
                return new Result<AtsmanagerAdminRegion>();
            }
        }
    }
}
