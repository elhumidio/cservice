using Domain.Entities;
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

        public async  Task<bool> AddPayment(ContractPayment payment)
        {
            var ret = _dataContext.Entry(payment).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            var ans = await _dataContext.SaveChangesAsync();
            return ans > 0;
        }
    }
}
