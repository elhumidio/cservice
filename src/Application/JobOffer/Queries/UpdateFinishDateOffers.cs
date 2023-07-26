using Application.Core;
using Domain.DTO.Location;
using Domain.Enums;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class UpdateFinishDateOffers
    {
        public class Query : IRequest<Result<bool>>
        {
            public int CompanyId { get; set; }
            
        }

        public class Handler : IRequestHandler<Query, Result<bool>>
        {
            private readonly IJobOfferRepository _jobOfferRepository;
            private readonly IContractProductRepository _contractProductRepository;
            private readonly IMediator _mediatr;

            public Handler(IJobOfferRepository jobOfferRepository,IContractProductRepository contractProductRepository,IMediator mediator)
            {
                _jobOfferRepository = jobOfferRepository;
                _contractProductRepository = contractProductRepository;
                _mediatr = mediator;    
            }

            public async Task<Result<bool>> Handle(Query request, CancellationToken cancellationToken)
            {
                var offers = _jobOfferRepository.GetOffersByCompanyId(request.CompanyId).ToList();
                try {
                    foreach (var offer in offers)
                    {
                        var productId = _contractProductRepository.GetIdProductByContract(offer.Idcontract);
                        var newFinishDate = await _mediatr.Send(new CalculateFinishDateOffer.Query
                        {
                            ContractID = offer.Idcontract,
                            ProductId = productId
                        });
                        offer.FinishDate = newFinishDate != null ? newFinishDate.Value : offer.FinishDate;
                        offer.Idstatus = (int)OfferStatus.Active;
                        var ret = await _jobOfferRepository.UpdateOffer(offer);
                    }

                    return Result<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    return Result<bool>.Failure(ex.Message);
                }
                
            }
        }
    }
}
