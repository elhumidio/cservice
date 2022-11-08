using Application.Contracts.Queries;
using Application.Core;
using Application.EnterpriseContract.DTO;
using Domain.DTO;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Application.EnterpriseContract.Queries
{
    public class GetCompanyInfoManagers
    {
        public class Query : IRequest<Result<AtsmanagerAdminRegion>>
        {
            public GetCompanyRequest Params { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<AtsmanagerAdminRegion>>
        {
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
                    IDSUser = _userRepository.GetUserIdByEmail(request.Params.Email)
                };
                var managers = _atSManagerAdminRepository.Get(request.Params.CompanyId);
                var ableManagers = new List<AtsmanagerAdminRegion>();
                var winnerManager = new AtsmanagerAdminRegion();
                if (managers != null)
                {

                    //get manager  by region or country
                    var managersByRegion = managers.Where(r => r.RegionId == request.Params.RegionId).ToList();
                    ableManagers.AddRange(managersByRegion);
                    if (!managersByRegion.Any()) {
                        var managersByCountry = managers.Where(r => r.CountryId == request.Params.CountryId).ToList();
                        ableManagers.AddRange(managersByCountry);
                    }
                    if (ableManagers.Any())
                    {
                        //assigned to the first with available units
                        foreach (var manager in ableManagers)
                        {
                            var result = await _mediatr.Send(new GetAvailableUnitsByOwner.Query
                            {
                                ContractId = request.Params.ContractId,
                                OwnerId = manager.ManagerId
                            });
                            var available = result.Value.FirstOrDefault(v => (int)v.type == request.Params.IdJobVacType);
                            if (available != null && available.Units > 0) {

                                winnerManager = manager;
                                return new Result<AtsmanagerAdminRegion> { Value = winnerManager };
                                
                            }
                        }
                    }
                    else
                    {
                        return new Result<AtsmanagerAdminRegion>();
                    }

                }
                return new Result<AtsmanagerAdminRegion>();
            }
        }
    }
}
