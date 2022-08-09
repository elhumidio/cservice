using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.JobOffer.Queries
{
    public class ListActivesByManager
    {

        public class Query : IRequest<Result<List<JobOfferDTO>>>
        {
            public int ContractID { get; set; }
            public int OwnerID { get; set; }

        }

        public class Handler : IRequestHandler<Query, Result<List<JobOfferDTO>>>
        {

            private readonly IJobOfferRepository _jobOffer;
            private readonly IContractProductRepository _contractProductRepo;
            private readonly IMapper _mapper;


            public Handler(IMapper mapper, IJobOfferRepository jobOffer, IContractProductRepository contractProductRepo)
            {
                _mapper = mapper;
                _jobOffer = jobOffer;
                _contractProductRepo = contractProductRepo;

            }

            public async Task<Result<List<JobOfferDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var isPack = _contractProductRepo.IsPack(request.ContractID);
                IQueryable<JobOfferDTO> query = null;
                if (isPack)
                {
                    query = _jobOffer.GetActiveOffersByContractAndManager(request.ContractID, request.OwnerID).ProjectTo<JobOfferDTO>(_mapper.ConfigurationProvider).AsQueryable();
                    return Result<List<JobOfferDTO>>.Success(await query.ToListAsync());
                }
                else
                {
                    query = _jobOffer.GetActiveOffersByContractAndManagerNoPack(request.ContractID, request.OwnerID).ProjectTo<JobOfferDTO>(_mapper.ConfigurationProvider).AsQueryable();
                    return Result<List<JobOfferDTO>>.Success(await query.ToListAsync());
                }
            }
        }
    }
}
