using Application.Cache;
using Application.Core;
using AutoMapper;
using Domain.EnterpriseDtos;
using Domain.DTO;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TURI.ContractService.Contracts.Contract.Services;
using TURI.ContractService.Contracts.Contract.Models.Response;

namespace Application.JobTitles.Command
{
    public class GetJobTitlesDenominationsFromActiveOffersCommandHandler : IRequestHandler<GetJobTitlesDenominationsFromActiveOffersCommand, Result<JobTitleDenominationsDto[]>>
    {
        private readonly IJobTitleDenominationsRepository _jobTitleRepository;
        private readonly ILogger<GetJobTitlesDenominationsFromActiveOffersCommandHandler> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IJobOfferService _jobOfferService;
        private readonly IMapper _mapper;

        public GetJobTitlesDenominationsFromActiveOffersCommandHandler(IJobTitleDenominationsRepository jobTitleRepository, ILogger<GetJobTitlesDenominationsFromActiveOffersCommandHandler> logger, IMemoryCache memoryCache, IJobOfferService jobOfferService,  IMapper mapper)
        {
            _jobTitleRepository = jobTitleRepository;
            _logger = logger;
            _mapper = mapper;
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _jobOfferService = jobOfferService;
        }

        public async Task<Result<JobTitleDenominationsDto[]>> Handle(GetJobTitlesDenominationsFromActiveOffersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<JobTitleDenominationsDto> jobTitlesDnominations = new List<JobTitleDenominationsDto>();
                string cacheKey = CacheKeys.JobTitlesDenominationsActiveOffersSpanish;

                switch (request.LanguageId)
                {
                    case (int)Languages.Spanish:
                        cacheKey = CacheKeys.JobTitlesDenominationsActiveOffersSpanish;
                        break;
                    case (int)Languages.English:
                        cacheKey = CacheKeys.JobTitlesDenominationsActiveOffersEnglish;
                        break;
                    case (int)Languages.Portuguese:
                        cacheKey = CacheKeys.JobTitlesDenominationsActiveOffersPortuguese;
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

                JobTitlesIdsResponseDTO titleslist = await _jobOfferService.GetActiveOffersJobTitlesIds();

                TitlesIdsDTO titles = _mapper.Map(titleslist, new TitlesIdsDTO());

                // Cache miss, fetch job titles denominations from the data source
                var jobTitlesList = await _jobTitleRepository.GetAllDenominationsActiveOffersByLanguage(request.LanguageId, titles);
               
                // Store job titles denominations in the cache with a specific expiration time (e.g., 30 minutes)
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
