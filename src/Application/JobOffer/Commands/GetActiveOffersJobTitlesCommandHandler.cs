using Application.Core;
using Domain.DTO;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.JobOffer.Commands
{
    public class GetActiveOffersJobTitlesCommandHandler : IRequestHandler<GetActiveOffersJobTitlesCommand, Result<JobTitlesIdsDTO>>
    {
        private readonly IJobOfferRepository _jobOffer;
        private readonly IMapper _mapper;
        private readonly ILogger<GetActiveOffersJobTitlesCommandHandler> _logger;

        public GetActiveOffersJobTitlesCommandHandler(IMapper mapper, IJobOfferRepository jobOffer, ILogger<GetActiveOffersJobTitlesCommandHandler> logger)
        {
            _mapper = mapper;
            _jobOffer = jobOffer;
            _logger = logger;
        }

        public async Task<Result<JobTitlesIdsDTO>> Handle(GetActiveOffersJobTitlesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<int?> jobtitlesIds = new List<int?>();

                jobtitlesIds = await _jobOffer.GetActiveOffersJobtitlesIds();

                JobTitlesIdsDTO dto = new JobTitlesIdsDTO();

                dto.TitlesIds = jobtitlesIds;

                return Result<JobTitlesIdsDTO>.Success(dto);
            }
            catch (Exception ex)
            {
                string error = $"message: Couldn't found active offers - Exception: {ex.Message} - {ex.InnerException}";
                _logger.LogError(error);
                return Result<JobTitlesIdsDTO>.Failure(error);
            }
        }
    }
}
