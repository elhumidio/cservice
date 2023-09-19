using Application.Core;
using Domain.DTO.ManageJobs;
using Domain.Repositories;
using MediatR;
using Application.Utils;

namespace Application.JobOffer.Commands
{
    public class GetCitiesByOffersCompanyCommand
    {
        public class Handler : IRequestHandler<GetCitiesQuery, Result<CitiesByOfferCompany>>
        {
            private readonly IJobOfferRepository _jobOfferRepository;

            public Handler(
                IJobOfferRepository jobOfferRepository
                )
            {
                _jobOfferRepository = jobOfferRepository;
            }

            public async Task<Result<CitiesByOfferCompany>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
            {
                var allOffers = _jobOfferRepository.GetAllOffersByCompany(request.CompanyId).DistinctBy(a => a.City).Select(b => b).OrderBy(c => c.City).ToList();
                CitiesByOfferCompany response = new();
                Dictionary<int, string> cities = new();
                int counter = 0;

                foreach (var offer in allOffers)
                {
                    if(!cities.ContainsValue(Utils.Extensions.CapitalizeStringUpperFirstLowerRest(offer.City.Trim() ?? string.Empty)))
                    cities.Add(counter, Utils.Extensions.CapitalizeStringUpperFirstLowerRest(offer.City.Trim() ?? string.Empty));
                    counter++;
                }
                response.Cities = cities;
                return Result<CitiesByOfferCompany>.Success(response);
            }
        }
    }

    
}
