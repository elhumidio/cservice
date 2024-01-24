using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.JobTitles.Queries
{
    public class GetAllJobTitles
    {
       
        public class GetAll : IRequest<Result<List<JobTitleDto>>>
        {
            public class Handler : IRequestHandler<GetAll, Result<List<JobTitleDto>>>
            {
                private readonly IJobTitleDenominationsRepository _jobTitleRepository;
                private readonly IMapper _mapper;

                public Handler(IJobTitleDenominationsRepository jobTitleRepository, IMapper mapper)
                {
                    _jobTitleRepository = jobTitleRepository;
                    _mapper = mapper;
                }

                public async Task<Result<List<JobTitleDto>>> Handle(GetAll dto, CancellationToken cancellationToken)
                {
                    try
                    {
                        var jobTitlesList = _jobTitleRepository.GetAll().ProjectTo<JobTitleDto>(_mapper.ConfigurationProvider).ToList();

                        return Result<List<JobTitleDto>>.Success(jobTitlesList);
                    }
                    catch (Exception ex)
                    {
                        var a = ex;
                        return null;
                    }
                }
            }
        }
    }
}
