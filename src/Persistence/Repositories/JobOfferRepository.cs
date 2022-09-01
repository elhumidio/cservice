using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories
{
    public class JobOfferRepository : IJobOfferRepository
    {
        private readonly DataContext _dataContext;
        private readonly ILogger _logger;

        public JobOfferRepository(DataContext dataContext,ILogger logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }
        public IQueryable<JobVacancy> GetActiveOffersByContract(int contractId)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Idcontract == contractId
                && !a.ChkDeleted
                && !a.ChkFilled
                && a.FinishDate >= DateTime.Today
                && a.Idstatus == (int)OfferStatus.Active);
            return query;
        }

        public IQueryable<JobVacancy> GetOffersByContract(int contractId)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Idcontract == contractId && a.Idstatus == (int)OfferStatus.Active);
            return query;
        }

        public IQueryable<JobVacancy> GetActiveOffersByContractAndManager(int contractId, int managerId)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Idcontract == contractId && !a.ChkDeleted
                && !a.ChkFilled
                && a.FinishDate >= DateTime.Today && a.IdenterpriseUserG == managerId && a.Idstatus == (int)OfferStatus.Active);

            return query;

        }

        public Task<int> UpdateOffer(JobVacancy jobUpdated)
        {
            try {
                var current = _dataContext.JobVacancies.Where(a => a.IdjobVacancy == jobUpdated.IdjobVacancy).FirstOrDefault();
                if (current != null)
                {
                    current = jobUpdated;
                    var ret = _dataContext.SaveChangesAsync();
                    return ret;
                }
                else return Task.FromResult(-1);
            }
            catch (Exception ex) {
                string message = $"Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}";
                _logger.LogError(message: message);
                return Task.FromResult(-1);
            }
            

        }

        public Task<int> FileOffer(JobVacancy job)
        {
            try {
                job.ChkFilled = true;
                job.FilledDate = DateTime.Now;
                job.ModificationDate = DateTime.Now;
                var ret = _dataContext.SaveChangesAsync();
                return ret;
            }
            catch (Exception ex) {
                string message = $"Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}";
                _logger.LogError(message: message);
                return Task.FromResult(-1);
            }
            
            
        }

        public JobVacancy GetOfferById(int id)
        {
            var offer = _dataContext.JobVacancies.Where(o => o.IdjobVacancy == id).FirstOrDefault();
            return offer;
        }

        public int Add(JobVacancy job)
        {
            try {
                var a = _dataContext.Add(job).Entity;
                var ret = _dataContext.SaveChanges();
                return job.IdjobVacancy;
            }
            catch (Exception ex) {
                string message = $"Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}";
                _logger.LogError(message: message);
                return -1;
            }
            
        }


        public IQueryable<JobVacancy> GetActiveOffersByContractAndManagerNoPack(int contractId, int managerId)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Idcontract == contractId
                 && a.IdenterpriseUserG == managerId && a.Idstatus == (int)OfferStatus.Active);

            return query;

        }

        public IQueryable<JobVacancy> GetActiveOffersByCompany(int enterpriseId)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Identerprise == enterpriseId
                && !a.ChkDeleted
                && !a.ChkFilled
                && a.FinishDate > DateTime.Today.AddDays(-1)
                && a.Idstatus == (int)OfferStatus.Active);

            return query;

        }
        public IQueryable<JobVacancy> GetActiveOffersByContractNoPack(int contractId)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Idcontract == contractId
                 && a.Idstatus == (int)OfferStatus.Active);
            return query;
        }

        public IQueryable<JobVacancy> GetActiveOffersByContractOwnerType(int contractId, int owner, int type)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Idcontract == contractId
            && a.IdenterpriseUserG == owner
            && a.IdjobVacType == type
            && !a.ChkDeleted
            && !a.ChkFilled
            && a.FinishDate >= DateTime.Today
            && a.Idstatus == (int)OfferStatus.Active);
            return query;
        }
        public IQueryable<JobVacancy> GetActiveOffersByContractOwnerTypeNoPack(int contractId, int owner, int type)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Idcontract == contractId
            && a.IdenterpriseUserG == owner
            && a.IdjobVacType == type
            && a.Idstatus == (int)OfferStatus.Active);
            return query;
        }

        public IQueryable<JobVacancy> GetConsumedUnitsWelcomeNotSpain(int companyId)
        {
            var res = _dataContext.JobVacancies
               .Join(_dataContext.Contracts, p => new { p.Idcontract }, jv => new { jv.Idcontract },
                   (jv, p) => new { jv, p })
               .Join(_dataContext.ContractProducts, jvc => jvc.p.Idcontract, cp => cp.Idcontract, (jvc, cp) => new { jvc, cp })
               .Where(o => o.jvc.jv.Identerprise == companyId

               && (o.cp.Idproduct == 110 || o.cp.Idproduct == 115)
               && !o.jvc.jv.ChkFilled && !o.jvc.jv.ChkDeleted && o.jvc.jv.Idstatus == 1 && o.jvc.jv.FinishDate >= DateTime.Today
               )
               .Select(o => o.jvc.jv);

            return res;
        }


    }
}
