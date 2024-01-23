using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.JobOffer.Commands
{
    public class GetActiveOffersJobTitlesCommandHandler : IRequestHandler<GetActiveOffersJobTitlesCommand, Result<List<int?>>>
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

        public async Task<Result<List<int?>>> Handle(GetActiveOffersJobTitlesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<int?> jobtitlesIds = new List<int?>();

                jobtitlesIds = await _jobOffer.GetActiveOffersJobtitlesIds();

                return Result<List<int?>>.Success(jobtitlesIds);
            }
            catch (Exception ex)
            {
                string error = $"message: Couldn't found active offers - Exception: {ex.Message} - {ex.InnerException}";
                _logger.LogError(error);
                return Result<List<int?>>.Failure(error);
            }
        }
    }
}
