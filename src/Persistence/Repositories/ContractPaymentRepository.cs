using Domain.Repositories;

namespace Persistence.Repositories
{
    public class ContractPaymentRepository : IContractPaymentRepository
    {
        private DataContext _dataContext;

        public ContractPaymentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public bool HasPayments(int contractId)
        {
            var payments = _dataContext.ContractPayments.Where(c => c.Idcontract == contractId).Count();
            return payments > 0;
                 
        }
    }
}
