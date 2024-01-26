using Domain.DTO;
using Domain.EnterpriseDtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence.Repositories
{
    public class JobTitleDenominationsRepository : IJobTitleDenominationsRepository
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<JobTitleDenominationsRepository> _logger;

        public JobTitleDenominationsRepository(DataContext dataContext, ILogger<JobTitleDenominationsRepository> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public List<JobTitleDenomination> GetAllForArea(int idArea, int idSite)
        {
            //NOTE: Select here **mandatory** as some link broken with EF, thinks a random column exists
            return _dataContext.JobTitlesDenominations.Select(a => new JobTitleDenomination
            {
                Id = a.Id,
                BaseName = a.BaseName,
                Denomination = a.Denomination,
                FkJobTitle = a.FkJobTitle,
                LanguageId = a.LanguageId,
                SiteMap = a.SiteMap
            })
                .Join(_dataContext.JobTitlesAreas,
                denom => denom.FkJobTitle,
                area => area.FkJobTitleId, (denom, area) => new
            {
                Denom = denom,
                IDArea = area.FkAreaId,
                IDSite = area.FkIdsite
            })
            .Where(v => v.IDArea == idArea && v.IDSite == idSite)
            .Select(d => d.Denom)
            .ToList();
        }

        public int GetAreaByJobTitle(int titleId)
        {
            var result = _dataContext.JobTitlesAreas
            .Where(v => v.FkJobTitleId == titleId)
            .FirstOrDefault();

            if (result == null)
                return 0;
            else
                return result.FkAreaId;
        }

        public JobTitleDenomination GetDefaultDenomination(int idJobTitle, int idSite)
        {
            int languageId;
            switch (idSite)
            {
                case 6: case 11: //Spain, Mexico
                    languageId = 7; break;
                case 8: case 18: //Portugal, Brazil
                    languageId = 17; break;
                default:
                    languageId = 14; break;
            }

            return _dataContext.JobTitlesDenominations.FirstOrDefault(d => d.Id == idJobTitle && d.LanguageId == languageId);
            
        }

        public IQueryable<JobTitle> GetAll()
        {
            var jobTitles = _dataContext.Titles;
            return jobTitles.AsQueryable();
        }

        public async Task<List<JobTitleDenominationsDto>> GetAllDenominations()
        {
            try
            {
                var jobTitlesBasicData = _dataContext.JobTitlesDenominations
                   .Join(_dataContext.Titles, d => d.FkJobTitle, a => a.Id, (d, a) => new { d, a })
                   .Select(jd => new
                   {
                       Id = jd.d.Id,
                       FkJobTitle = jd.a.Id,
                       Denomination = jd.d.Denomination,
                       LanguageId = jd.d.LanguageId,
                       BaseName = jd.d.BaseName,
                       Isco08 = jd.a.Isco08.Trim(),
                       Isco88 = jd.a.Isco88.Trim()
                   })
                   .ToList();

                // Fetching job titles area IDs
                var jobTitlesAreaIds = _dataContext.JobTitlesAreas
                    .Select(jta => new { jta.FkJobTitleId, jta.FkAreaId })
                    .ToList();

                // Mapping area IDs to job titles
                var jobTitles = jobTitlesBasicData.Select(jd => new JobTitleDenominationsDto
                {
                    Id = jd.Id,
                    Denomination = jd.Denomination,
                    FkJobTitle = jd.FkJobTitle,
                }).ToList();

                return jobTitles.ToList();
            }
            catch (Exception ex)
            {
                string message = $"Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}";
                _logger.LogError(message: message);
                return null;
            }

        }

        public async Task<List<JobTitleDenominationsDto>> GetAllDenominationsByLanguage(int languageId)
        {
            try
            {
                var jobTitlesBasicData = _dataContext.JobTitlesDenominations
                   .Join(_dataContext.Titles, d => d.FkJobTitle, a => a.Id, (d, a) => new { d, a })
                   .Select(jd => new
                   {
                       Id = jd.d.Id,
                       FkJobTitle = jd.a.Id,
                       Denomination = jd.d.Denomination,
                       LanguageId = jd.d.LanguageId,
                       BaseName = jd.d.BaseName,
                       Isco08 = jd.a.Isco08.Trim(),
                       Isco88 = jd.a.Isco88.Trim()
                   })
                   .Where(a => a.LanguageId == languageId)
                   .ToList();

                // Fetching job titles area IDs
                var jobTitlesAreaIds = _dataContext.JobTitlesAreas
                    .Select(jta => new { jta.FkJobTitleId, jta.FkAreaId })
                    .ToList();

                // Mapping area IDs to job titles
                var jobTitles = jobTitlesBasicData.Select(jd => new JobTitleDenominationsDto
                {
                    Id = jd.Id,
                    Denomination = jd.Denomination,
                    FkJobTitle = jd.FkJobTitle,
                }).ToList();

                return jobTitles.ToList();
            }
            catch (Exception ex)
            {
                string message = $"Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}";
                _logger.LogError(message: message);
                return null;
            }

        }

        public async Task<List<JobTitleDenominationsDto>> GetAllDenominationsActiveOffersByLanguage(int languageId)
        {
            try
            {
                var titles = _dataContext.JobVacancies
                 .Where(a =>
                        !a.ChkDeleted
                        && !a.ChkFilled
                        && a.FinishDate >= DateTime.Today
                        && a.Idstatus == (int)OfferStatus.Active
                        && a.TitleId != null
                        && a.TitleId > 0)
                .Select(t => t.TitleId).Distinct();

                var jobTitlesBasicData = _dataContext.JobTitlesDenominations
                   .Join(_dataContext.Titles, d => d.FkJobTitle, a => a.Id, (d, a) => new { d, a })
                   .Where(a => a.d.LanguageId == languageId && titles.Contains(a.d.FkJobTitle))
                   .Select(jd => new JobTitleDenominationsDto
                   {
                       Id = jd.d.Id,
                       FkJobTitle = jd.a.Id,
                       Denomination = jd.d.Denomination,
                   })
                   .ToList();


                return jobTitlesBasicData;
            }
            catch (Exception ex)
            {
                string message = $"Message: {ex.Message} - InnerException: {ex.InnerException} - StackTrace: {ex.StackTrace}";
                _logger.LogError(message: message);
                return null;
            }

        }
    }
}
