using Application.Interfaces;
using Domain.DTO;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Aimwel.Queries
{
    public class VerifyOfferGoal
    {
        public class Verifier : IRequest<VerifyGoalsOfferResponse>
        {
            public VerifyGoalsRequest OffersToVerify { get; set; }
        }

        public class Handler : IRequestHandler<Verifier, VerifyGoalsOfferResponse>
        {
            private readonly ICampaignsManagementRepository _campaignsManagementRepo;
            private readonly IJobOfferRepository _jobOfferRepository;
            private readonly IAreaRepository _areaRepo;
            private readonly IRegionRepository _regionRepository;
            private readonly IApplicationServiceLocal _applicationServiceLocal;

            public Handler(ICampaignsManagementRepository campaignsManagementRepository,
                IJobOfferRepository jobOfferRepository,
                IApplicationServiceLocal applicationServiceLocal, IAreaRepository areaRepository, IRegionRepository regionRepository)
            {
                _campaignsManagementRepo = campaignsManagementRepository;
                _jobOfferRepository = jobOfferRepository;
                _areaRepo = areaRepository;
                _regionRepository = regionRepository;
                _applicationServiceLocal = applicationServiceLocal;
            }

            public async Task<VerifyGoalsOfferResponse> Handle(Verifier request, CancellationToken cancellationToken)
            {
                var response = new VerifyGoalsOfferResponse();
                var listRedirectOffers = new HashSet<JobVacancy>();
                var listAppliesOffers = new HashSet<JobVacancy>();
                var listAppliedRequest = new ListOffersRequest();
                var listRedirectRequest = new ListOffersRequest();
                List<JobVacancy> actualFeed = new();
                var settings = _campaignsManagementRepo.GetAllSettings().ToList();
                var offerIds = request.OffersToVerify.OffersList;

                var offers = _jobOfferRepository.GetOffersByIds(offerIds); // Obtener todas las ofertas de una vez

                foreach (var offer in offers)
                {
                    if (string.IsNullOrWhiteSpace(offer.ExternalUrl))
                    {
                        listAppliesOffers.Add(offer);
                    }
                    else
                    {
                        listRedirectOffers.Add(offer);
                    }
                }

                listAppliedRequest.Offers.AddRange(listAppliesOffers.Select(a => a.IdjobVacancy).ToList());
                var offerApplicants = await _applicationServiceLocal.CountApplicantsByOffers(listAppliedRequest);
                listRedirectRequest.Offers.AddRange(listRedirectOffers.Select(a => a.IdjobVacancy).ToList());
                var offerRedireccions = await _applicationServiceLocal.CountRedirectsByOffer(listRedirectRequest);

                //try with nuget method

                foreach (var o in listAppliesOffers)
                {
                    if (o.Identerprise == 27966)
                        continue;
                    else
                    {
                        actualFeed.Add(o);
                        var setting = settings.FirstOrDefault(a => a.AreaId == o.Idarea);
                        var retrievedApplicants = offerApplicants.results.FirstOrDefault(of => of.jobId == o.IdjobVacancy) == null ? 0
                            : offerApplicants.results.FirstOrDefault(of => of.jobId == o.IdjobVacancy).Applicants;
                        if (retrievedApplicants < setting.Goal)
                        {
                            response.GoalsOffersList.Add(new GoalsOffer { OfferId = o.IdjobVacancy, ReachedGoals = false });
                        }
                        else
                        {
                            response.GoalsOffersList.Add(new GoalsOffer { OfferId = o.IdjobVacancy, ReachedGoals = true });
                        }
                    }
                }

                foreach (var o in listRedirectOffers)
                {
                    if (o.Identerprise == 27966)
                        continue;
                    else
                    {
                        actualFeed.Add(o);
                        var setting = settings.FirstOrDefault(a => a.AreaId == o.Idarea);
                        var retrievedRedirections = offerRedireccions.results.FirstOrDefault(of => of.jobId == o.IdjobVacancy) == null ? 0 :

                            offerRedireccions.results.FirstOrDefault(of => of.jobId == o.IdjobVacancy).Applicants;

                        if (retrievedRedirections < setting?.Goal)
                        {
                            response.GoalsOffersList.Add(new GoalsOffer { OfferId = o.IdjobVacancy, ReachedGoals = false });
                        }
                        else
                        {
                            response.GoalsOffersList.Add(new GoalsOffer { OfferId = o.IdjobVacancy, ReachedGoals = true });
                        }
                    }
                }
                var offersGroupedByAreaAndregion = actualFeed.GroupBy(obj => new { obj.Idarea, obj.Idregion });
                List<FeedsAggregatorsLog> list = new List<FeedsAggregatorsLog>();
                var id = _campaignsManagementRepo.GetNextId();
                foreach (var item in offersGroupedByAreaAndregion)
                {
                    var obj = new FeedsAggregatorsLog
                    {
                        Id = id,
                        TotalOffers = item.Count(),
                        Date = DateTime.Now,
                        AreaId = item.Key.Idarea,
                        RegionId = item.Key.Idregion,
                        RegionName = _regionRepository.GetRegionNameByID(item.Key.Idregion, false),
                        AreaName = _areaRepo.GetAreaName(item.Key.Idarea)
                    };
                    list.Add(obj);
                }
                await _campaignsManagementRepo.SaveFeedLogs(list);
                return response;
            }
        }
    }
}
