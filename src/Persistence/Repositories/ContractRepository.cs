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
    }
}
