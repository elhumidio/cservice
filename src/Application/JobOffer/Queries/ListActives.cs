using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffer.Queries
{
    public class ListActives
    {

        public class Query : IRequest<Result<List<JobOfferDto>>>
        {
            public int ContractID { get; set; }

        }

        public class Handler : IRequestHandler<Query, Result<List<JobOfferDto>>>
        {

            private readonly IJobOfferRepository _jobOffer;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, IJobOfferRepository jobOffer)
            {
                _mapper = mapper;
                _jobOffer = jobOffer;

            }

            public async Task<Result<List<JobOfferDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _jobOffer.GetActiveOffersByContract(request.ContractID).ProjectTo<JobOfferDto>(_mapper.ConfigurationProvider).AsQueryable();
                return Result<List<JobOfferDto>>.Success(
                    await query.ToListAsync()
                );
            }


        }
    }
}
