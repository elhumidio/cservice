using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JobOffer.Queries
{
    public class ListActivesWholeDataByCompany
    {
      
            public class Query : IRequest<Result<List<JobOfferWholeDto>>>
            {
                public int CompanyId { get; set; }
            }

            public class Handler : IRequestHandler<Query, Result<List<JobOfferWholeDto>>>
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

                public async Task<Result<List<JobOfferWholeDto>>> Handle(Query request, CancellationToken cancellationToken)
                {
                    IQueryable<JobOfferWholeDto> query = null;
                    query = _jobOffer.GetActiveOffersByCompany(request.CompanyId).ProjectTo<JobOfferWholeDto>(_mapper.ConfigurationProvider).AsQueryable();
                    return Result<List<JobOfferWholeDto>>.Success(await query.ToListAsync());
                }
            }
        
    }
}
