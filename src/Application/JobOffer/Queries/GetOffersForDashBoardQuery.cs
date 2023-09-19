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

        [DataMember]
        public string? Location { get; set; }

        [DataMember]
        public int BrandId { get; set; }

        [DataMember]
        public string? Title { get; set; }
    }

    public class Handler : IRequestHandler<GetOffersForDashBoardQuery, Result<ManageJobsDto>>
    {
        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly IApplicationService _applicationService;
        private readonly IContractRepository _contractRepository;
        private readonly IMapper _mapper;
        private readonly IApiUtils _utils;
        private const string JOIN = "ç";

        public Handler(IMapper mapper,
            IJobOfferRepository jobOfferRepository,
            IApplicationService applicationService,
            IApiUtils apiUtils,
            IContractRepository contractRepository)
        {
            _mapper = mapper;
            _jobOfferRepository = jobOfferRepository;
            _applicationService = applicationService;
            _utils = apiUtils;
            _contractRepository = contractRepository;
        }

        public async Task<Result<ManageJobsDto>> Handle(GetOffersForDashBoardQuery request, CancellationToken cancellationToken)
        {
            var response = new ManageJobsDto();
            var offers = await _jobOfferRepository.GetOffersForActionDashboard(_mapper.Map(request, new ManageJobsArgs()));
            if (!string.IsNullOrEmpty(request.Title))
            {
                offers = offers.Where(o => o.Title.Contains(request.Title) || o.Title == request.Title).ToList();
            }
            if (request.BrandId != 0)
            {
                offers = offers.Where(o => o.Idbrand == request.BrandId).ToList();
            }
            if (!string.IsNullOrEmpty(request.Location))
            {
                offers = offers.Where(o => o.City == request.Location || o.City.Contains(request.Location) || request.Location.Contains(o.City)).ToList();
            }

            var filedOffersResponse = offers.Where(o => o.ChkFilled).ToList();
            var activesOfferResponse = offers.Where(o => !o.ChkDeleted && !o.ChkFilled && o.FinishDate.Date >= DateTime.Today).ToList();
            response.Offers = request.Actives ? activesOfferResponse : request.Filed ? filedOffersResponse : request.All ? offers : null;
            response.Offers = response.Offers.Skip(request.PageSize * (request.Page - 1)).Take(request.PageSize).ToList();

            var counters = await _applicationService.GetCandidatesByOffersByState(new CandidatesStatesByOffersRequest { OfferIds = response.Offers.Select(a => a.IdjobVacancy).ToList() });

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
                offer.CanSeeFilters = await _contractRepository.IsAllowedContractForSeeingFilters(offer.Idcontract);

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
