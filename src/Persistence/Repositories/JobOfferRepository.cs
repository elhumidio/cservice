using Domain.Classes;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
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
                && a.FinishDate >= DateTime.Today && a.IdenterpriseUserG == managerId && (a.Idstatus == (int)OfferStatus.Active || a.Idstatus == (int)OfferStatus.Pending));

            return query;
        }

        public async Task<int> UpdateOffer(JobVacancy jobUpdated)
        {
            try
            {
                var current = _dataContext.JobVacancies.Where(a => a.IdjobVacancy == jobUpdated.IdjobVacancy).FirstOrDefault();
                if (current != null)
                {
                    current = jobUpdated;
                    var ret = await _dataContext.SaveChangesAsync();
                    return ret;
                }
                else return -1;
            }
            catch (Exception ex)
            {
                string message = $"Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}";
                _logger.LogError(message: message);
                return -1;
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
                job.FilledDate = DateTime.Now;  
                job.ModificationDate= DateTime.Now; 
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
            try
            {
                var offer = _dataContext.JobVacancies.FirstOrDefault(o => o.IdjobVacancy == id);
                if (offer == null)
                    return new JobVacancy();
                else return offer;
            }
            catch (Exception ex)
            {
                return new JobVacancy();
            }
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

        public IQueryable<JobVacancy> GetActiveOffers()
        {
            var query = _dataContext.JobVacancies.Where(a => !a.ChkDeleted
            && !a.ChkFilled
            && a.FinishDate >= DateTime.Today.Date
            && a.Idstatus == (int)OfferStatus.Active
            && !a.ChkDeleted
            && (a.Idsite == (int)Sites.SPAIN || a.Idsite == (int)Sites.PORTUGAL));
            return query;
        }

        public List<int>? GetoffersinCampaigns()
        {
            var offers = _dataContext.CampaignManagements.Select(c => (int)c.IdjobVacancy).ToList();
            return offers;
        }

        public IQueryable<JobVacancy> GetActiveOffersByContractAndManagerNoPack(int contractId, int managerId)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Idcontract == contractId
                 && a.IdenterpriseUserG == managerId && a.Idstatus != (int)OfferStatus.Deleted);

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
            && a.Idstatus != (int)OfferStatus.Deleted);
            return query;
        }

        public IQueryable<JobVacancy> GetActiveOffersByContractOwnerTypeNoPack(int contractId, int owner, int type)
        {
            var query = _dataContext.JobVacancies.Where(a => a.Idcontract == contractId
            && a.IdenterpriseUserG == owner
            && a.IdjobVacType == type
            && a.Idstatus != (int)OfferStatus.Deleted);
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
               // && !o.jvc.jv.ChkFilled && !o.jvc.jv.ChkDeleted && o.jvc.jv.Idstatus == 1 && o.jvc.jv.FinishDate >= DateTime.Today
               )
               .Select(o => o.jvc.jv);

            return res;
        }

        public async Task<IReadOnlyList<JobDataDefinition>> GetActiveJobs(int maxActiveDays)
        {
            var maxActiveDaysDate = DateTime.Now.AddDays(-maxActiveDays).Date;

            var query = (from job in _dataContext.JobVacancies
                         join brand in _dataContext.Brands on job.Idbrand equals brand.Idbrand
                         where !job.ChkDeleted
                                && !job.ChkFilled
                                && job.PublicationDate < DateTime.Now
                                && job.FinishDate > DateTime.Now
                                && job.Idstatus == (int)OfferStatus.Active
                                && job.PublicationDate > maxActiveDaysDate
                         select new JobDataDefinition()
                         {
                             Title = job.Title,
                             CompanyName = brand.Name,
                             IDCountry = job.Idcountry,
                             IDRegion = job.Idregion,
                             IDArea = job.Idarea,
                             IDJobVacancy = job.IdjobVacancy,
                             IDEnterprise = job.Identerprise,
                             IDSite = job.Idsite,
                             PublicationDate = job.PublicationDate,
                             IDCity = job.Idcity ?? 0,
                             ChkBlindVac = job.ChkBlindVac
                             //Description = "",
                         });

            return await query.ToListAsync();
        }

        public async Task<JobDataDefinition?> GetJobDataById(int id)
        {
            var query = (from job in _dataContext.JobVacancies
                         join brand in _dataContext.Brands on job.Idbrand equals brand.Idbrand
                         where job.IdjobVacancy == id
                         select new JobDataDefinition()
                         {
                             Title = job.Title,
                             CompanyName = brand.Name,
                             IDCountry = job.Idcountry,
                             IDRegion = job.Idregion,
                             IDArea = job.Idarea,
                             IDJobVacancy = job.IdjobVacancy,
                             IDEnterprise = job.Identerprise,
                             IDSite = job.Idsite,
                             PublicationDate = job.PublicationDate,
                             IDCity = job.Idcity ?? 0,
                             //Description = "",
                         });

            var offer = await query.FirstAsync();
            return offer;
        }

        public Task<List<FeaturedJob>> GetFeaturedJobs()
        {
            var query = (from featuredJob in _dataContext.FeaturedJobs
                         select featuredJob);

            return query.ToListAsync();
        }

        public async Task<List<JobVacancy>> GetOffersCreatedLastTwoDays()
        {
            var offers = await _dataContext.JobVacancies
                .Where(o => o.Idstatus == (int)OfferStatus.Active
                && !o.ChkFilled
                && !o.ChkDeleted
                && o.FinishDate.Date > DateTime.Today.Date
                && o.PublicationDate.Date > DateTime.Today.Date.AddDays(-2))
                .OrderByDescending(a => a.FilledDate).ThenBy(a => a.IdjobVacancy).ToListAsync();
            return offers;
        }

        public async Task<List<JobVacancy>> GetInactiveOffersChunk()
        {
            var offers = await _dataContext.JobVacancies
                    .Where(o => (o.Idstatus != 1
                    || o.ChkFilled
                    || o.ChkDeleted
                    || o.FinishDate.Date > DateTime.Today.Date)
                    )
                    .OrderByDescending(a => a.FilledDate).Take(100).ToListAsync();
            return offers;
        }

        public Task<int> CountOffersPublished(int days)
        {
            var lastLogin = DateTime.Now.AddDays(-days).Date;

            var query = _dataContext.JobVacancies.Where(a => a.PublicationDate >= lastLogin).CountAsync();
            return query;
        }
    }
}
