using Application.Core;
using AutoMapper;
using Domain.DTO.ManageJobs;
using Domain.Repositories;
using MediatR;
using TURI.ApplicationService.Contracts.Application.Services;
using TURI.ApplicationService.Contracts.Application.Models;
using TURI.ApplicationService.Contracts.Application.Enums;

namespace Application.JobOffer.Queries
{
    public class GetOffersForDashBoardQuery : IRequest<Result<List<OfferModel>>>
    {
        public int SiteId { get; set; }
        public int CompanyId { get; set; }
        public int LanguageId { get; set; }
    }

    public class Handler : IRequestHandler<GetOffersForDashBoardQuery, Result<List<OfferModel>>>
    {
        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;

        public Handler(IMapper mapper,
            IJobOfferRepository jobOfferRepository,
            IApplicationService applicationService)
        {
            _mapper = mapper;
            _jobOfferRepository = jobOfferRepository;
            _applicationService = applicationService;
        }

        public async Task<Result<List<OfferModel>>> Handle(GetOffersForDashBoardQuery request, CancellationToken cancellationToken)
        {
            var offers = await  _jobOfferRepository.GetOffersForActionDashboard(request.CompanyId, request.SiteId, request.LanguageId);
            var counters = await _applicationService.GetCandidatesByOffersByState(new CandidatesStatesByOffersRequest { OfferIds = offers.Select(a => a.IdjobVacancy).ToList() });
            foreach (var offer in offers)
            {
                offer.NPendientes = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.Pending).FirstOrDefault()?.Count ?? 0;
                offer.NDescartados = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.Rejected).FirstOrDefault()?.Count ?? 0;
                offer.NEvaluating = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.Evaluating).FirstOrDefault()?.Count ?? 0;
                offer.NNuevos = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.New  ).FirstOrDefault()?.Count ?? 0;
                offer.NFinalistas = counters.Where(c => c.OfferId == offer.IdjobVacancy && c.Status == CandidateStatus.Finalist).FirstOrDefault()?.Count ?? 0;
            }
            return Result<List<OfferModel>>.Success(offers);
            
  
        }
    }
}
