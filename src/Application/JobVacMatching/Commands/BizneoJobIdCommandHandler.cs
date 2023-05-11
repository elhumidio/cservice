using Application.Core;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.JobVacMatching.Commands
{
    public class BizneoJobIdCommandHandler : IRequestHandler<BizneoJobIdCommand, Result<string>>
    {
        private readonly ILogger<BizneoJobIdCommandHandler> _logger;
        private readonly IRegJobVacMatchingRepository _regJobMatching;
        public BizneoJobIdCommandHandler(ILogger<BizneoJobIdCommandHandler> logger, IRegJobVacMatchingRepository regJobMatching)
        {
            _logger = logger;
            _regJobMatching = regJobMatching;
        }

        public async Task<Result<string>> Handle(BizneoJobIdCommand request, CancellationToken cancellationToken)
        {
            var regJobVac = _regJobMatching.GetAtsIntegrationInfoByJobId(request.JobVacancyID).Result;
            string jobId = string.Concat(regJobVac.IdjobVacancy,"-", regJobVac.ExternalId);
            return Result<string>.Success(jobId);
        }
    }
}
