using Application.Core;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class VerifyOffer
    {

        public class Query : IRequest<Result<int>>
        {
            public string ExternalId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<int>>
        {
            private readonly IRegJobVacMatchingRepository _matchingRepo;
            private readonly IJobOfferRepository _jobOffer;


            public Handler(IMapper mapper, IJobOfferRepository jobOffer, IRegJobVacMatchingRepository matchingRepo)
            {
                _jobOffer = jobOffer;
                _matchingRepo = matchingRepo;
            }

            public async Task<Result<int>> Handle(Query request, CancellationToken cancellationToken)
            {
                int idJobVacancy = 0;
                var atsMatching = await _matchingRepo.GetAtsIntegrationInfo(request.ExternalId);
                if (atsMatching != null)
                {
                    idJobVacancy = atsMatching.IdjobVacancy;
                }
                return Result<int>.Success(idJobVacancy);
            }

        }
    }
}
