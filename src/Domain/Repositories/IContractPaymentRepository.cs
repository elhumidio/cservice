namespace Domain.Repositories
{
    public interface IContractPaymentRepository
    {
        public bool HasPayments(int contractId);
    }
}
