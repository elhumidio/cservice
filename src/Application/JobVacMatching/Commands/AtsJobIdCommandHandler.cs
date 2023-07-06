using Application.Core;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.JobVacMatching.Commands
{
    public class AtsJobIdCommandHandler : IRequestHandler<AtsJobIdCommand, Result<string>>
    {
        private readonly ILogger<AtsJobIdCommandHandler> _logger;
        private readonly IRegJobVacMatchingRepository _regJobMatching;
        public AtsJobIdCommandHandler(ILogger<AtsJobIdCommandHandler> logger, IRegJobVacMatchingRepository regJobMatching)
        {
            _logger = logger;
            _regJobMatching = regJobMatching;
        }

        public async Task<Result<string>> Handle(AtsJobIdCommand request, CancellationToken cancellationToken)
        {
            var regJobVac = _regJobMatching.GetAtsIntegrationInfoByJobId(request.JobVacancyID).Result;
            return Result<string>.Success(regJobVac.ExternalId);
        }
    }
}
