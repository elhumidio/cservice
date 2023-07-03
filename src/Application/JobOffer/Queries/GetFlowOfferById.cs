using Application.Core;
using Application.JobOffer.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JobOffer.Queries
{
    public class GetFlowOfferById
    {
        public class Query : IRequest<Result<FlowOfferDTO>>
        {
            public int OfferId { get; set; }
            public int LanguageId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<FlowOfferDTO>>
        {
            private readonly IJobOfferRepository _jobOffer;
            private readonly IRegionRepository _regionOffer;
            private readonly IEnterpriseRepository _enterpriseOffer;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOffer, IRegionRepository regionOffer, IEnterpriseRepository enterpriseOffer)
            {
                _mapper = mapper;
                _jobOffer = jobOffer;
                _regionOffer = regionOffer;
                _enterpriseOffer = enterpriseOffer;
            }

            public async Task<Result<FlowOfferDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var offer = _jobOffer.GetOfferById(request.OfferId);
                if (offer == null)
                {
                    return Result<FlowOfferDTO>.Failure($"Error trying to get offer {request.OfferId}.");
                }
                FlowOfferDTO flowOffer = new FlowOfferDTO();
                flowOffer.Title = offer.Title;
                flowOffer.Location = _regionOffer.GetRegionNameByID(offer.Idregion, request.LanguageId == 14);
                flowOffer.CompanyName = _enterpriseOffer.GetCompanyNameCheckingBlind(offer.Identerprise, offer.ChkBlindVac);
                flowOffer.AreaId = offer.Idarea;
                flowOffer.CountryId = offer.Idcountry;
                flowOffer.RegionId = offer.Idregion;
                flowOffer.QuestId = offer.Idquest.GetValueOrDefault();
                flowOffer.SiteId = offer.Idsite;
                if (!string.IsNullOrEmpty(offer.ExternalUrl))
                {
                    flowOffer.ExternalURL = offer.ExternalUrl;
                }
                return Result<FlowOfferDTO>.Success(flowOffer);
            }
        }
    }
}
