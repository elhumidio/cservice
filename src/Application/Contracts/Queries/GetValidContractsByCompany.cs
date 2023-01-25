using Application.Contracts.DTO;
using Application.Core;
using Application.EnterpriseContract.Queries;
using Application.Utils;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class GetValidContractsByCompany
    {
        public class Get : IRequest<Result<List<ContractsDistDto>>>
        {
            public int CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Get, Result<List<ContractsDistDto>>>
        {
            private readonly IContractProductRepository _contractProductRepo;
            private readonly IContractRepository _contractRepository;
            private readonly IMediator _mediator;

            public Handler(IContractRepository contractRepository,
                IContractProductRepository contractProductRepository,
                IMediator mediator)
            {
                _contractProductRepo = contractProductRepository;
                _contractRepository = contractRepository;
                _mediator = mediator;   
            }

            public async Task<Result<List<ContractsDistDto>>> Handle(Get request, CancellationToken cancellationToken)
            {
                var list = new List<AvailableUnitsDto>();
                AvailableUnitsDto dto;
                var clist = new List<ContractsDistDto>();
                var company = await _mediator.Send(new GetCompanyInfoById.Query
                {
                    CompanyId = request.CompanyId
                });
                var contracts = await _contractRepository.GetValidContracts(request.CompanyId, company.Value.SiteId, ApiUtils.GetTuriLanguageBySite(company.Value.SiteId));
        

                foreach (var c in contracts)
                {
                    if (!clist.Exists(con => con.ContractId == c.ContractId))
                        clist.Add(c);
                }

                foreach (var cl in clist)
                {
                    var reg =await _contractRepository.GetWithReg(cl.ContractId);
                    int[] standards = { 0, 2, 4, 5, 10 };     
                    cl.TotalStandardUnits = reg.FirstOrDefault(u => standards.Contains(u.IdjobVacType)).Units;
                    cl.TotalFeaturedUnits = reg.FirstOrDefault(u => !standards.Contains(u.IdjobVacType)).Units;
                    var units =await _mediator.Send(new GetAvailableUnits.Query { ContractId = cl.ContractId });
                    cl.TotalAvailablestandard = units.Value.Where(u => standards.Contains((int)u.type)).First().Units;
                    cl.TotalAvailableFeatured = units.Value.Where(u => !standards.Contains((int)u.type)).First().Units;
                    cl.IsPack = _contractProductRepo.IsPack(cl.ContractId);
                }

                return Result<List<ContractsDistDto>>.Success(clist);
            }
        }
    }
}
