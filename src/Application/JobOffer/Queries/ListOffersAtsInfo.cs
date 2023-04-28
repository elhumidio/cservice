using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using Domain.DTO;
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
    public class ListOffersAtsInfo
    {
        public class Get : IRequest<Result<List<OfferMinInfoAtsDto>>>
        {
            public string ExternalId { get; set; }
            public int CompanyId { get; set; }
        }

        public class Handler : IRequestHandler<Get, Result<List<OfferMinInfoAtsDto>>>
        {
            private readonly IJobOfferRepository _jobOffer;
            
            

            public Handler(IMapper mapper, IJobOfferRepository jobOffer)
            {                
                _jobOffer = jobOffer;            
            }

            public async Task<Result<List<OfferMinInfoAtsDto>>> Handle(Get dto, CancellationToken cancellationToken)
            {          
                var query =await _jobOffer.GetOfferInfoByExternalId(dto.ExternalId, dto.CompanyId);
                return Result<List<OfferMinInfoAtsDto>>.Success(query);
            }
        }
    }
}
