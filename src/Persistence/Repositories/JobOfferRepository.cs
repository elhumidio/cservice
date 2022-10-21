using Domain.Classes;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories
{
    public class JobOfferRepository : IJobOfferRepository
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<JobOfferRepository> _logger;

        public JobOfferRepository(DataContext dataContext, ILogger<JobOfferRepository> logger)
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
            try
            {
                var current = _dataContext.JobVacancies.Where(a => a.IdjobVacancy == jobUpdated.IdjobVacancy).FirstOrDefault();
                if (current != null)
                {
                    current = jobUpdated;
                    var ret = _dataContext.SaveChangesAsync();
                    return ret;
                }
                else return Task.FromResult(-1);
            }
            catch (Exception ex)
            {
                string message = $"Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}";
                _logger.LogError(message: message);
                return Task.FromResult(-1);
            }
        }

        public int FileOffer(JobVacancy job)
        {
            try
            {
                job.ChkFilled = true;
                job.FilledDate = DateTime.Now;
                job.ModificationDate = DateTime.Now;
                var ret = _dataContext.SaveChanges();
                return ret;
            }
            catch (Exception ex)
            {
                string message = $"Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}";
                _logger.LogError(message: message);
                return -1;
            }
        }

        public int DeleteOffer(JobVacancy job)
        {
            try
            {
                job.ChkFilled = true;
                job.ChkDeleted = true;
                job.FinishDate = DateTime.Now;
                job.Idstatus = (int)OfferStatus.Deleted;
                var ret = _dataContext.SaveChanges();
                return ret;
            }
            catch (Exception ex)
            {
                string message = $"Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}";
                _logger.LogError(message: message);
                return -1;
            }
        }

        public JobVacancy GetOfferById(int id)
        {
            var offer = _dataContext.JobVacancies.Where(o => o.IdjobVacancy == id).FirstOrDefault();
            return offer;
        }

        /// <summary>
        /// It Returns aimwel id by idjobvacancy
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public string AimwelIdByJobId(int jobId)
        {
            string aimwelId = string.Empty;
            var offer = _dataContext.JobVacancies.Where(v => v.IdjobVacancy == jobId).FirstOrDefault();
            if (offer != null)
                aimwelId = offer.AimwelCampaignId;
            return aimwelId;
        }

        public int Add(JobVacancy job)
        {
            try
            {
                var a = _dataContext.Add(job).Entity;
                var ret = _dataContext.SaveChanges();
                return job.IdjobVacancy;
            }
            catch (Exception ex)
            {
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

        public IQueryable<JobVacancy> GetActiveOffersByContractAndType(int contractId, int type)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Idcontract == contractId
            && a.IdjobVacType == type
            && !a.ChkDeleted
            && !a.ChkFilled
            && a.FinishDate >= DateTime.Today
            && a.Idstatus == (int)OfferStatus.Active);
            return query;
        }

        public IQueryable<JobVacancy> GetActiveOffersByContractAndTypeNoPack(int contractId, int type)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Idcontract == contractId
            && a.IdjobVacType == type
            && a.Idstatus == (int)OfferStatus.Active);
            return query;
        }

        public IQueryable<JobVacancy> GetActiveOffersByContractOwnerTypeNoPack(int contractId, int owner, int type)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Idcontract == contractId
            && a.IdenterpriseUserG == owner
            && a.IdjobVacType == type
            && a.FinishDate >= DateTime.Today
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

        /*
        public IQueryable<JobVacancy> GetAllJobs(int countryId)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Identerprise == 28463
                && !a.ChkDeleted
                && !a.ChkFilled
                && a.FinishDate > DateTime.Today.AddDays(-1)
                && a.Idstatus == (int)OfferStatus.Active);

            return query;
        }
        */

        public async Task<IQueryable<JobData>> GetAllJobs()
        {
            var semanal = DateTime.Now.AddDays(-7).Date;
            var catstr = "";
            var catstrEn = "";
            var _enLanguageID = 14;

            var query = (from job in _dataContext.JobVacancies
                         join enterprise in _dataContext.Enterprises on job.Identerprise equals enterprise.Identerprise
                         join brand in _dataContext.Brands on job.Idbrand equals brand.Idbrand
                         where !job.ChkDeleted
                                && !job.ChkFilled
                                && job.PublicationDate.AddDays(14) >= DateTime.Now
                                && job.Idstatus == (int)OfferStatus.Active
                         select new JobData()
                         {
                             Title = job.Title,
                             CompanyName = brand.Name,
                             IDCountry = job.Idcountry,
                             IDRegion = job.Idregion,
                             IDArea = job.Idarea,
                             IDJobVacancy = job.IdjobVacancy,
                             IDBrand = job.Idbrand,
                             IDEnterprise = job.Identerprise,
                             ChkBlindVacancy = job.ChkBlindVac,
                             PublicationDate = job.PublicationDate,
                             City = job.City,
                             IDCity = (job.Idcity.HasValue) ? job.Idcity.Value : 0
                         });

            //LOCATION = job.IDRegion == 61 ? country.BaseName : region.BaseName + ", " + country.BaseName,
            //CATEGORY = area.BaseName,
            //CATEGORY_EN = areaEn.BaseName,
            //LOCATION_EN = job.IDRegion == 61 ? country.BaseName : regionEn.BaseName + ", " + country.BaseName,
            //CATEGORY_MORE_JOBS = catstr + area.BaseName,
            //CATEGORY_MORE_JOBS_EN = catstrEn + areaEn.BaseName,

            return (IQueryable<JobData>)query;
        }
    }
}
