using Application.Core;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.JobTitles.Command
{
    public class GetAllJobTitlesDenominationsCommandHandler : IRequestHandler<GetAllJobTitlesDenominationsCommand, Result<JobTitleDenominationsDto[]>>
    {
        private readonly IJobTitleDenominationsRepository _jobTitleRepository;

        public GetAllJobTitlesDenominationsCommandHandler(IJobTitleDenominationsRepository jobTitleRepository)
        {
            _jobTitleRepository = jobTitleRepository;
        }

        public async Task<Result<JobTitleDenominationsDto[]>> Handle(GetAllJobTitlesDenominationsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var jobTitlesList = await _jobTitleRepository.GetAllDenominations();

                return Result<JobTitleDenominationsDto[]>.Success(jobTitlesList.ToArray());
            }
            catch (Exception ex)
            {
                var a = ex;
                return Result<JobTitleDenominationsDto[]>.Success(Array.Empty<JobTitleDenominationsDto>());
            }

        }
    }
}
