using Application.Core;
using Application.JobOffer.DTO;
using Application.Utils;
using AutoMapper;
using Domain.DTO.ManageJobs;
using Domain.DTO.Requests;
using Domain.Repositories;
using MediatR;
using System.Runtime.Serialization;
using TURI.ApplicationService.Contracts.Application.Enums;
using TURI.ApplicationService.Contracts.Application.Models;
using TURI.ApplicationService.Contracts.Application.Services;

namespace Application.JobOffer.Queries
{
    [DataContract]
    public class GetOffersForDashBoardQuery : IRequest<Result<ManageJobsDto>>
    {
        [DataMember]
        public int CompanyId { get; set; }

        [DataMember]
        public int Site { get; set; }

        [DataMember]
        public int LangId { get; set; }

        [DataMember]
        public bool Actives { get; set; }

        [DataMember]
        public bool Filed { get; set; }

        [DataMember]
        public bool All { get; set; }

        [DataMember]
        public int Page { get; set; } = 1;

        [DataMember]
        public int PageSize { get; set; } = 10;
    }

    public class Handler : IRequestHandler<GetOffersForDashBoardQuery, Result<ManageJobsDto>>
    {
        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;
        private readonly IApiUtils _utils;
        private const string JOIN = "ç";

        public Handler(IMapper mapper,
            IJobOfferRepository jobOfferRepository,
            IApplicationService applicationService, IApiUtils apiUtils)
        {
            _mapper = mapper;
            _jobOfferRepository = jobOfferRepository;
            _applicationService = applicationService;
            _utils = apiUtils;
        }

        public async Task<Result<ManageJobsDto>> Handle(GetOffersForDashBoardQuery request, CancellationToken cancellationToken)
        {
            var response = new ManageJobsDto();
            var offers = await _jobOfferRepository.GetOffersForActionDashboard(_mapper.Map(request, new ManageJobsArgs()));
            var filedOffersResponse = offers.Where(o => o.ChkFilled).ToList();
            var activesOfferResponse = offers.Where(o => !o.ChkDeleted && !o.ChkFilled && o.FinishDate.Date >= DateTime.Today).ToList();
            response.Offers = request.Actives ? activesOfferResponse : request.Filed ? filedOffersResponse : request.All ? offers : null;
            response.Offers = response.Offers.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize).ToList();
            var counters = await _applicationService.GetCandidatesByOffersByState(new CandidatesStatesByOffersRequest { OfferIds = response.Offers.Select(a => a.IdjobVacancy).ToList() });
            response.Filed = filedOffersResponse.Count;
            response.Actives = activesOfferResponse.Count;
            response.Total = offers.Count;
            foreach (var offer in response.Offers)
            {
                offer.NPendientes = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.Pending).FirstOrDefault()?.Count ?? 0;
                offer.NDescartados = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.Rejected).FirstOrDefault()?.Count ?? 0;
                offer.NEvaluating = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.Evaluating).FirstOrDefault()?.Count ?? 0;
                offer.NNuevos = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.New).FirstOrDefault()?.Count ?? 0;
                offer.NFinalistas = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.Finalist).FirstOrDefault()?.Count ?? 0;
                offer.OfferUrl = _utils.BuildURLJobvacancy(offer);
                offer.Caducity = (int)(offer.FinishDate.Date - DateTime.Now.Date).TotalDays;
                offer.FormData = $"{offer.IdjobVacancy}{JOIN}{offer.Idcontract}{JOIN}{offer.IdjobVacType}{JOIN}{offer.Caducity}{JOIN}{offer.ChkFilled.ToString()}";
                offer.JobRegType = _utils.GetRegTypeBySiteAndLanguage(request.LangId, offer.IdjobRegType);
                IsOldOfferArgs getCVExpierdDays = new()
                {
                    OfferPublicationDate = offer.PublicationDate,
                    OfferCheckPack = offer.ChkPack,
                    ContractStartDate = offer.ContractStartDate,
                    ContractFinishDate = offer.ContractFinishDate,
                    OfferIDJobVacType = offer.IdjobVacType,
                    ProductDuration = 60,
                    ExtensionDays = offer.ExtensionDays
                };
                offer.IsOldOffer = _utils.GetCVExpiredDays(getCVExpierdDays) > 0;
            }
            return Result<ManageJobsDto>.Success(response);
        }
    }
}
