using Domain.Entities;

namespace Domain.Repositories
{
    public interface IContractPaymentRepository
    {
        public bool HasPayments(int contractId);
        public Task<bool> AddPayment(ContractPayment payment);
    }
}
