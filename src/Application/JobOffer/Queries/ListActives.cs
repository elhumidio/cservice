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

        public class Query : IRequest<Result<List<JobOfferDTO>>>
        {
            public int ContractID { get; set; }

        }

        public class Handler : IRequestHandler<Query, Result<List<JobOfferDTO>>>
        {

            private readonly IJobOfferRepository _jobOffer;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, IJobOfferRepository jobOffer)
            {
                _mapper = mapper;
                _jobOffer = jobOffer;

            }

            public async Task<Result<List<JobOfferDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _jobOffer.GetActiveOffersByContract(request.ContractID).ProjectTo<JobOfferDTO>(_mapper.ConfigurationProvider).AsQueryable();
                return Result<List<JobOfferDTO>>.Success(
                    await query.ToListAsync()
                );
            }


        }
    }
}
