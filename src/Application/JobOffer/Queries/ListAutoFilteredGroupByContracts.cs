using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
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
    public class ListAutoFilteredGroupByContracts
    {
        public class Query : IRequest<Result<List<OffersGroupedByContractsDto>>>
        {
            public List<int> ContractIDs { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<List<OffersGroupedByContractsDto>>>
        {
            private readonly IJobOfferRepository _jobOffer;
            private readonly IContractProductRepository _contractProductRepo;
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;   

            public Handler(IMapper mapper,
                IJobOfferRepository jobOffer,
                IContractProductRepository contractProductRepo,
                IMediator mediator)
            {
                _mapper = mapper;
                _jobOffer = jobOffer;
                _contractProductRepo = contractProductRepo;
                _mediator = mediator;
            }

            public async Task<Result<List<OffersGroupedByContractsDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<OffersGroupedByContractsDto> list = new List<OffersGroupedByContractsDto>(); 
                foreach (var contract in request.ContractIDs)
                {
                    OffersGroupedByContractsDto dto = new OffersGroupedByContractsDto();
                    dto.ContractId = contract;
                    var ListOffers = await _mediator.Send(new ListAutoFiltered.Query {

                         ContractID = contract
                    });
                    dto.ListOffers = ListOffers.Value;
                    list.Add(dto);
                }
                return Result<List<OffersGroupedByContractsDto>>.Success(list);              
               
            }
        }
    }
}
