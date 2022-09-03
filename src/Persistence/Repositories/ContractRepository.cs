using Domain.DTO;
using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly DataContext _dataContext;
        
        public ContractRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IQueryable<Contract> Get(int contractId)
        {
            var query = _dataContext.Contracts.Where(a => a.Idcontract == contractId);
            return query;
        }

        public IQueryable<Contract> GetContracts(int companyId)
        {
            var query = _dataContext.Contracts.Where(c => c.Identerprise == companyId && c.ChkApproved && !c.ChkCancel && c.FinishDate >= DateTime.Today).OrderByDescending(con => con.StartDate);
            return query;
        }

        public bool IsValidContract(int contractId)
        {
            var contracts = _dataContext.Contracts
                .Where(c => c.FinishDate >= DateTime.Today && c.ChkApproved && c.Idcontract == contractId && !c.ChkCancel)
                .OrderBy(con => con.FinishDate);
            return contracts != null && contracts.Any();
        }
        public IQueryable<ServiceTypeDto> GetServiceTypes(int contractId)
        {
            IQueryable<ServiceTypeDto> list;
            list = _dataContext.Contracts
                .Join(_dataContext.ContractProducts, c => new { c.Idcontract }, cp => new { cp.Idcontract }, (c, cp) => new { c, cp })
                .Join(_dataContext.Products, p => p.cp.Idproduct, pr => pr.Idproduct, (p, pr) => new { p, pr })
                .Join(_dataContext.ProductLines, pl => pl.pr.Idproduct, ppl => ppl.Idproduct, (pl, ppl) => new { pl, ppl })
                .Where( a => a.ppl.IdserviceType != null && a.pl.p.c.Idcontract == contractId)
                .Select(res => new ServiceTypeDto  {  IDContract  = res.pl.p.c.Idcontract, ServiceType=  (int)res.ppl.IdserviceType }).Distinct().AsQueryable();

            return list;

        }
       
    }
}
