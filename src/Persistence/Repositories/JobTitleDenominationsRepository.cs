using Domain.DTO;
using Domain.EnterpriseDtos;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
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
            return _dataContext.JobTitleDenominations.Join(_dataContext.JobTitleAreas, inner => inner.FK_JobTitle, outer => outer.FK_JobTitleID, (inner, outer) => new
            {
                Denom = inner,
                IDArea = outer.FK_AreaID,
                IDSite = outer.FK_IDSite
            })
            .Where(v => v.IDArea == idArea && v.IDSite == idSite)
            .Select(d => d.Denom)
            .ToList();
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

            return _dataContext.JobTitleDenominations.FirstOrDefault(d => d.ID == idJobTitle && d.LanguageId == languageId);
            
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
                var jobTitlesBasicData = _dataContext.JobTitleDenominations
                   .Join(_dataContext.Titles, d => d.FK_JobTitle, a => a.Id, (d, a) => new { d, a })
                   .Select(jd => new
                   {
                       Id = jd.d.ID,
                       FkJobTitle = jd.a.Id,
                       Denomination = jd.d.Denomination,
                       LanguageId = jd.d.LanguageId,
                       BaseName = jd.d.BaseName,
                       Isco08 = jd.a.Isco08.Trim(),
                       Isco88 = jd.a.Isco88.Trim()
                   })
                   .ToList();

                // Fetching job titles area IDs
                var jobTitlesAreaIds = _dataContext.JobTitleAreas
                    .Select(jta => new { jta.FK_JobTitleID, jta.FK_AreaID })
                    .ToList();

                // Mapping area IDs to job titles
                var jobTitles = jobTitlesBasicData.Select(jd => new JobTitleDenominationsDto
                {
                    Id = jd.Id,
                    Denomination = jd.Denomination,
                    LanguageId = jd.LanguageId,
                    BaseName = jd.BaseName,
                    FkJobTitle = jd.FkJobTitle,
                    Isco08 = jd.Isco08,
                    Isco88 = jd.Isco88,
                    JobTitlesAreas = jobTitlesAreaIds.Where(jta => jta.FK_JobTitleID == jd.FkJobTitle)
                                                       .Select(jta => jta.FK_AreaID)
                                                       .ToList()
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
                var jobTitlesBasicData = _dataContext.JobTitleDenominations
                   .Join(_dataContext.Titles, d => d.FK_JobTitle, a => a.Id, (d, a) => new { d, a })
                   .Select(jd => new
                   {
                       Id = jd.d.ID,
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
                var jobTitlesAreaIds = _dataContext.JobTitleAreas
                    .Select(jta => new { jta.FK_JobTitleID, jta.FK_AreaID })
                    .ToList();

                // Mapping area IDs to job titles
                var jobTitles = jobTitlesBasicData.Select(jd => new JobTitleDenominationsDto
                {
                    Id = jd.Id,
                    Denomination = jd.Denomination,
                    LanguageId = jd.LanguageId,
                    BaseName = jd.BaseName,
                    FkJobTitle = jd.FkJobTitle,
                    Isco08 = jd.Isco08,
                    Isco88 = jd.Isco88,
                    JobTitlesAreas = jobTitlesAreaIds.Where(jta => jta.FK_JobTitleID == jd.FkJobTitle)
                                                       .Select(jta => jta.FK_AreaID)
                                                       .ToList()
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

                var jobTitlesBasicData = _dataContext.JobTitleDenominations
                   .Join(_dataContext.Titles, d => d.FK_JobTitle, a => a.Id, (d, a) => new { d, a })
                   .Select(jd => new JobTitleDenominationsDto
                   {
                       Id = jd.d.ID,
                       FkJobTitle = jd.a.Id,
                       Denomination = jd.d.Denomination,
                       LanguageId = jd.d.LanguageId,
                       BaseName = jd.d.BaseName,
                       Isco08 = jd.a.Isco08.Trim(),
                       Isco88 = jd.a.Isco88.Trim()
                   })
                   .Where(a => a.LanguageId == languageId && titles.Contains(a.FkJobTitle))
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
