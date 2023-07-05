using Application.Core;
using MediatR;

namespace Application.JobVacMatching.Commands
{
    public class AtsJobIdCommand : IRequest<Result<string>>
    {
        public int JobVacancyID { get; set; }
    }
}
