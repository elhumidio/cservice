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
            public List<int> OffersToVerify {  get; set; }  
        }
        public class Handler : IRequestHandler<Verifier, VerifyGoalsOfferResponse>
        {
            private readonly ICampaignsManagementRepository _campaignsManagementRepo;
            private readonly IJobOfferRepository _jobOfferRepository;
            private readonly IApplicationService _applicationService;
            

            public Handler(ICampaignsManagementRepository campaignsManagementRepository,
                IJobOfferRepository jobOfferRepository,
                IApplicationService applicationService)
            {
                _applicationService = applicationService;
                _campaignsManagementRepo = campaignsManagementRepository;
                _jobOfferRepository = jobOfferRepository;   
                    
            }

            public async Task<VerifyGoalsOfferResponse> Handle(Verifier request, CancellationToken cancellationToken)
            {
                var response = new VerifyGoalsOfferResponse();
                var listRedirectOffers = new HashSet<JobVacancy>();
                var listAppliesOffers = new HashSet<JobVacancy>();
                var listAppliedRequest = new ListOffersRequest();
                var listRedirectRequest = new ListOffersRequest();
                var settings = _campaignsManagementRepo.GetAllSettings().ToList();
                var offerIds = request.OffersToVerify;

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
                var offerApplicants = await _applicationService.CountApplicantsByOffers(listAppliedRequest);
                listRedirectRequest.Offers.AddRange(listRedirectOffers.Select(a => a.IdjobVacancy).ToList());
                var offerRedireccions = await _applicationService.CountRedirectsByOffer(listRedirectRequest);


                foreach (var o in listAppliesOffers)
                {
                    if (o.Identerprise == 27966)
                        continue;
                    else
                    {
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
 
                return response;
            }
        }
    }
}
