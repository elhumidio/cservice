using Application.Core;
using Application.Utils;
using AutoMapper;
using Domain.DTO.ManageJobs;
using Domain.DTO.Requests;
using Domain.Repositories;
using MediatR;
using TURI.ApplicationService.Contracts.Application.Enums;
using TURI.ApplicationService.Contracts.Application.Models;
using TURI.ApplicationService.Contracts.Application.Services;

namespace Application.JobOffer.Queries
{
    public class GetOffersForDashBoardQuery : IRequest<Result<List<OfferModel>>>
    {
        public int CompanyId { get; set; }
        public int Site { get; set; }
        public int LangId { get; set; }
        public bool Actives { get; set; }
        public bool Filed { get; set; }
        public bool All { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;                
        
    }

    public class Handler : IRequestHandler<GetOffersForDashBoardQuery, Result<List<OfferModel>>>
    {
        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;
        private readonly IApiUtils _utils;

        public Handler(IMapper mapper,
            IJobOfferRepository jobOfferRepository,
            IApplicationService applicationService, IApiUtils apiUtils)
        {
            _mapper = mapper;
            _jobOfferRepository = jobOfferRepository;
            _applicationService = applicationService;
            _utils = apiUtils;
        }
        public async Task<Result<List<OfferModel>>> Handle(GetOffersForDashBoardQuery request, CancellationToken cancellationToken)
        {
            var offers = await _jobOfferRepository.GetOffersForActionDashboard(_mapper.Map(request, new ManageJobsArgs()));
            var counters = await _applicationService.GetCandidatesByOffersByState(new CandidatesStatesByOffersRequest { OfferIds = offers.Select(a => a.IdjobVacancy).ToList() });
            foreach (var offer in offers)
            {
                offer.NPendientes = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.Pending).FirstOrDefault()?.Count ?? 0;
                offer.NDescartados = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.Rejected).FirstOrDefault()?.Count ?? 0;
                offer.NEvaluating = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.Evaluating).FirstOrDefault()?.Count ?? 0;
                offer.NNuevos = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.New).FirstOrDefault()?.Count ?? 0;
                offer.NFinalistas = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.Finalist).FirstOrDefault()?.Count ?? 0;
                offer.OfferUrl = _utils.BuildURLJobvacancy(offer);
                
            }
            return Result<List<OfferModel>>.Success(offers);
        }
    }
}
