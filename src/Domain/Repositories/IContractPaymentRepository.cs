using Domain.DTO;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IContractPaymentRepository
    {
        public bool HasPayments(int contractId);
        public Task<bool> AddPayment(ContractPayment payment);
        public Task<ContractPayment> GetPaymentByContractId(int contractId);
        public Task<bool> UpdatePayment(ContractPayment payment);
        public Task<List<ContractPaymentDto>> GetPaymentsByContractId(int contractId);
        public List<int> GetWelcomeContractsDueOfferNotPayed();

    }
}
