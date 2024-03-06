using Domain.DTO;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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
            var payments = _dataContext.ContractPayments.Where(c => c.Idcontract == contractId  && (bool)c.Finished).Count();
            return payments > 0;
                 
        }

        public List<int> GetWelcomeContractsDueOfferNotPayed()
        {
            List<int> contractList = new List<int>();
            contractList = (from c in _dataContext.Contracts
                       join p in _dataContext.ContractPayments on c.Idcontract equals p.Idcontract
                       join cp in _dataContext.ContractProducts on c.Idcontract equals cp.Idcontract
                       join v in _dataContext.JobVacancies on c.Idcontract equals v.Idcontract
                       where cp.Idproduct == 110
                       && c.ChkApproved
                       && (!(bool)p.Finished || p.Finished == null)
                       && v.FinishDate.Date == DateTime.Today.AddDays(-1)
                       select c.Idcontract).ToList();

            return contractList;

        }

        public async Task<ContractPayment> GetPaymentByContractId(int contractId)
        {
           
                var payment = await _dataContext.ContractPayments.Where(c => c.Idcontract == contractId).FirstOrDefaultAsync();
                return payment;
        }
        public async Task<List<ContractPaymentDto>> GetPaymentsByContractId(int contractId)
        {

            var payments = await _dataContext.ContractPayments.Where(c => c.Idcontract == contractId).ToListAsync();
            var list = payments.Select(a => new ContractPaymentDto
            {
                 ConvertRate = a.ConvertRate,
                 Currency = a.Currency,
                 DataPayment = a.DataPayment,
                 Finished = a.Finished,
                 Idcontract = a.Idcontract,
                 IdcontractPayment = a.IdcontractPayment,
                 Payment = a.Payment,
                 PaymentWithoutTax = a.PaymentWithoutTax,
                 TaxAmount = a.TaxAmount,
                 CouponDiscount = a.CouponDiscount
            }).ToList();
            return list;
        }


        public async  Task<bool> AddPayment(ContractPayment payment)
        {
            try {
                var ret = _dataContext.Entry(payment).State = EntityState.Added;
                var ans = await _dataContext.SaveChangesAsync();
                return ans > 0;
            }
            catch(Exception ex)
            {
                var a = ex;
            }
            return false;
        }

        public async Task<bool> UpdatePayment(ContractPayment payment)
        {
            //update payment
            var ret = _dataContext.Entry(payment).State = EntityState.Modified;
            var ans = await _dataContext.SaveChangesAsync();
            return ans > 0;
        }
    }
}
