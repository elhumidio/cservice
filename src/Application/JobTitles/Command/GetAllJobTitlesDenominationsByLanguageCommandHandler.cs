using Application.Cache;
using Application.Core;
using Domain.DTO;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Application.JobTitles.Command
{
    public class GetAllJobTitlesDenominationsByLanguageCommandHandler : IRequestHandler<GetAllJobTitlesDenominationsByLanguageCommand, Result<JobTitleDenominationsDto[]>>
    {
        private readonly IJobTitleDenominationsRepository _jobTitleRepository;
        private readonly ILogger<GetAllJobTitlesDenominationsByLanguageCommandHandler> _logger;
        private readonly IMemoryCache _memoryCache;
       
        public GetAllJobTitlesDenominationsByLanguageCommandHandler(IJobTitleDenominationsRepository jobTitleRepository, ILogger<GetAllJobTitlesDenominationsByLanguageCommandHandler> logger, IMemoryCache memoryCache)
        {
            _jobTitleRepository = jobTitleRepository;
            _logger = logger;
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<Result<JobTitleDenominationsDto[]>> Handle(GetAllJobTitlesDenominationsByLanguageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<JobTitleDenominationsDto> jobTitlesDnominations = new List<JobTitleDenominationsDto>();

                string cacheKey = CacheKeys.JobTitlesDenominationsSpanish;

                switch (request.LanguageId)
                {
                    case (int)Languages.Spanish:
                        cacheKey = CacheKeys.JobTitlesDenominationsSpanish;
                        break;
                    case (int)Languages.English:
                        cacheKey = CacheKeys.JobTitlesDenominationsEnglish;
                        break;
                    case (int)Languages.Portuguese:
                        cacheKey = CacheKeys.JobTitlesDenominationsPortuguese;
                        break;
                    default:
                        break;

                }
                var cachedDenominations = _memoryCache.TryGetValue(cacheKey, out jobTitlesDnominations);

                // Try to get job titles denominations from the cache
                if (cachedDenominations)
                {
                    // Cache hit, return job titles from the cache
                    return Result<JobTitleDenominationsDto[]>.Success(jobTitlesDnominations.ToArray()); ;
                }

                // Cache miss, fetch job titles denominations from the data source
                var jobTitlesList = await _jobTitleRepository.GetAllDenominationsByLanguage(request.LanguageId);

                // Store job titles denominations in the cache with a specific expiration time (e.g., 60 minutes)
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                };

                _memoryCache.Set(cacheKey, jobTitlesList, cacheEntryOptions);

                return Result<JobTitleDenominationsDto[]>.Success(jobTitlesList.ToArray());
            }
            catch (Exception ex)
            {
                string error = $"message: Couldn't found denominations - Exception: {ex.Message} - {ex.InnerException}";
                _logger.LogError(error);
                return Result<JobTitleDenominationsDto[]>.Failure(error);
            }

        }
    }
}
