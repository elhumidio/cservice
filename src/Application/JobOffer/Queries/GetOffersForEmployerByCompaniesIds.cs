using API.DataContext;
using Application.Core;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using System.Linq;
using System.Text.RegularExpressions;
using TURI.ApplicationService.Contracts.Application.Models;
using TURI.ApplicationService.Contracts.Application.Services;
using TURI.ContractService.Contracts.Contract.Models.Response;

namespace Application.JobOffer.Queries
{
    public class GetOffersForEmployerByCompaniesIds
    {
        public class Get : IRequest<Result<List<KeyValuesResponse>>>
        {
            public List<int> CompaniesIds { get; set; }
            public int? State { get; set; }
            public DateTime? MaxDate { get; set; }

        }

        public class Handler : IRequestHandler<Get, Result<List<KeyValuesResponse>>>
        {
            private readonly IJobOfferRepository _jobOfferRepository;
            private readonly IApplicationService _applicationService;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IJobOfferRepository jobOffer, IApplicationService applicationService)
            {
                _mapper = mapper;
                _jobOfferRepository = jobOffer;
                _applicationService = applicationService;
            }

            public async Task<Result<List<KeyValuesResponse>>> Handle(Get request, CancellationToken cancellationToken)
            {
                var query = _jobOfferRepository.GetActiveOffersByCompaniesIds(request.CompaniesIds);
                List<int> offersInts = query.Where(u => u.IdjobVacancy > 0).Select(u => u.IdjobVacancy).ToList();
                string offersIntsd = string.Join<int>(",", offersInts);
                Dictionary<int, List<int>> managersOffers = query.Where(u => u.IdenterpriseUserG > 0)
                    .GroupBy(u => (int)u.IdenterpriseUserG)
                    .ToDictionary(u => u.Key, u => u.Select(j => j.IdjobVacancy).ToList());

                List<ManagerApplicationsResponse> offerApplicationsPending = (await _applicationService.GetApplicationsByOffersIds(new ListOffersIdsRequest { OffersIds = offersInts, State = request.State, MaxDate = request.MaxDate })).ToList();

                List<KeyValuesResponse> managersApplications = new List<KeyValuesResponse>();
                foreach (var item in managersOffers)
                {
                    managersApplications.Add(new KeyValuesResponse()
                    {
                        Id = item.Key,
                        Value = offerApplicationsPending.Where(a => item.Value.Contains(a.Key)).Sum(a => a.ItemsCount),
                    });
                }

                //for testing:
                //select eu.identerprise, j.IDEnterpriseUserG, count(*) from TRegistration r
                //INNER JOIN TJobVacancy j on j.IDJobVacancy = r.IDJobVacancy
                //INNER JOIN TEnterpriseUser eu on eu.IDEnterpriseUser = j.IDEnterpriseUserG
                //where
                //r.IDRegistrationState = 1
                //AND r.RegistrationDate >= '2023-06-07T21:37:02.580'
                //AND ChkFilled = 0
                //AND chkDeleted = 0
                // and convert(date, FinishDate) >= convert(date, getdate())
                //GROUP BY eu.identerprise, j.IDEnterpriseUserG
                //ORDER BY eu.identerprise, j.IDEnterpriseUserG

                return Result<List<KeyValuesResponse>>.Success(managersApplications);
            }
        }
    }
}
