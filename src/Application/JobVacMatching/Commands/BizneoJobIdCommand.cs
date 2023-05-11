using Application.Core;
using MediatR;

namespace Application.JobVacMatching.Commands
{
    public class BizneoJobIdCommand : IRequest<Result<string>>
    {
        public int JobVacancyID { get; set; }
    }
}
