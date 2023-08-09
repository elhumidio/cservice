using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories
{
    public class SalesforceTransactionRepository : ISalesforceTransactionRepository
    {
        private readonly DataContext _dataContext;

        public SalesforceTransactionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public int Add(SalesforceTransaction tran)
        {
            var a = _dataContext.Add(tran).Entity;
            _dataContext.SaveChanges();
            return tran.IdsalesforceTransaction;
        }

        public SalesforceTransaction Get(int idTransaction)
        {
            var trans = _dataContext.SalesforceTransactions.FirstOrDefault(t => t.IdsalesforceTransaction == idTransaction);
            return trans;
        }

        public void Update(SalesforceTransaction tran)
        {
            var current = _dataContext.SalesforceTransactions.Where(a => a.IdsalesforceTransaction == tran.IdsalesforceTransaction).FirstOrDefault();
            if (current != null)
            {
                current = tran;
                var ret = _dataContext.SaveChanges();
            }
        }
    }
}
