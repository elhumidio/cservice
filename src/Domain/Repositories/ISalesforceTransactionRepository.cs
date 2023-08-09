using Domain.Entities;

namespace Domain.Repositories
{
    public interface ISalesforceTransactionRepository
    {
        public int Add(SalesforceTransaction tran);

        public void Update(SalesforceTransaction tran);

        public SalesforceTransaction Get(int idTransaction);
    }
}
