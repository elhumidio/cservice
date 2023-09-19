using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JobOffer.Queries.CampaignsQueries
{
    public class GetCampaignManagement
    {
        public class Query : IRequest<Result<CampaignsManagement>>
        {
            public int IDJobVacancy { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<CampaignsManagement>>
        {
            private readonly ICampaignsManagementRepository _campaignsManagementRepository;            
            

            public Handler(IMapper mapper,ICampaignsManagementRepository campaignsManagementRepository  )
            {
                
                _campaignsManagementRepository = campaignsManagementRepository;
            }

            public async Task<Result<CampaignsManagement>> Handle(Query request, CancellationToken cancellationToken)
            {
                var management =await _campaignsManagementRepository.GetCampaignManagement(request.IDJobVacancy);
                return Result<CampaignsManagement>.Success(management);
            }
        }
    }
}
