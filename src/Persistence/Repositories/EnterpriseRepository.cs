using Domain.Repositories;
using API.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class EnterpriseRepository : IEnterpriseRepository
    {
        private DataContext _dataContext;

        public EnterpriseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool IsRightCompany(int enterpriseId)
        {
            var enterprises = _dataContext.Enterprises.Where(e => e.Identerprise == enterpriseId
            && e.ChkActive != null
            && e.ChkActive == true);
            //&& e.Idstatus == (int)EnterpriseStatus.Active);
            return enterprises != null && enterprises.Any();
        }

        public bool UpdateATS(int enterpriseId)
        {
            var company = _dataContext.Enterprises.Where(e => e.Identerprise == enterpriseId).FirstOrDefault();
            if (company != null)
                company.Ats = true;
            _dataContext.SaveChanges();
            return company != null;
        }

        public int GetSite(int companyId)
        {
            int site = 0;
            var company = _dataContext.Enterprises.Where(e => e.Identerprise == companyId && e.ChkActive == true).FirstOrDefault();
            if (company != null)
            {
                site = (int)company.SiteId;
            }
            return site;
        }

        public API.DataContext.Enterprise Get(int companyId) {

            var company = _dataContext.Enterprises.FirstOrDefault(c => c.Identerprise == companyId);
            if (company != null)
                return company;
            else return null;
        }

        public string GetCompanyName(int companyId)
        {
            string CorporateName = string.Empty;
            var name = _dataContext.Enterprises.Where(c => c.Identerprise == companyId).FirstOrDefault().CorporateName;
            if (!string.IsNullOrEmpty(name))
                CorporateName = name;
            return CorporateName;
        }

        public string GetCompanyNameCheckingBlind(int companyId, bool isBlind)
        {
            string CorporateName = string.Empty;
            if (isBlind)
            {
                var enterpriseBlind = _dataContext.EnterpriseBlinds.Where(c => c.Identerprise == companyId).FirstOrDefault();
                if (enterpriseBlind != null)
                {
                    CorporateName = enterpriseBlind.Name;
                }
            }
            else
            {
                var item = _dataContext.Enterprises.Where(c => c.Identerprise == companyId).FirstOrDefault();
                if(item != null)
                {
                    var name = item.Name;
                    if (!string.IsNullOrEmpty(name))
                        CorporateName = name;
                }
            }
            return CorporateName;
        }

        public string GetCompanyDescriptionCheckingBlind(int companyId, bool isBlind)
        {
            string CorporateDescription = string.Empty;
            if (isBlind)
            {
                var enterpriseBlind = _dataContext.EnterpriseBlinds.Where(c => c.Identerprise == companyId).FirstOrDefault();
                if (enterpriseBlind != null)
                {
                    CorporateDescription = enterpriseBlind.Description;
                }
            }
            else
            {
                var description = _dataContext.Enterprises.Where(c => c.Identerprise == companyId).FirstOrDefault().Description;
                if (!string.IsNullOrEmpty(description))
                {
                    CorporateDescription = description;
                }
            }
            return CorporateDescription;
        }

        public string GetCompanyNameByBrandId(int brandId)
        {
            string CorporateName = string.Empty;
            var name = _dataContext.Brands.Where(c => c.Idbrand == brandId).FirstOrDefault().Name;
            if (!string.IsNullOrEmpty(name))
                CorporateName = name;
            return CorporateName;
        }

        public int GetCompanyRegion(int companyId)
        {
            var region = _dataContext.Enterprises.Where(c => c.Identerprise == companyId).FirstOrDefault().Idregion;
            return region;
        }

        public Task<int> GetCountCompaniesActive()
        {
            var query = _dataContext.Enterprises.Where(e => e.ChkActive == true).CountAsync();
            return query;
        }

        public List<int> GetCompaniesWithActiveJobs()
        {
            var query = _dataContext.JobVacancies
                .Where(vac =>
                    vac.ChkFilled == false &&
                    vac.ChkDeleted == false &&
                    vac.ChkBlindVac == false &&
                    vac.FinishDate < DateTime.Now)
                .Select(vac => vac.Identerprise)
                .Distinct()
                .ToList();

            return query;
        }

        public string GetCompanyLogo(int companyId, int brandId, bool isBlind)
        {
            string logoURL = "https://www.turijobs.com/static/img/global/nologo.png";
            // Offer blind then return static logo.
            if (isBlind)
            {
                return logoURL;
            }
            // Searching logo.
            var logo = _dataContext.Logos.FirstOrDefault(x => x.Identerprise == companyId && x.Idbrand == brandId);
            // Return enterprise logo by brand.
            if (logo != null && !string.IsNullOrEmpty(logo.UrlImgBig))
            {
                logoURL = logo.UrlImgBig;
            }
            return logoURL;
        }
    }
}
