using Application.Core;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.JobOffer.Queries
{
    public class CalculateFinishDateOffer
    {
        public class Query : IRequest<Result<DateTime>>
        {
            public int ContractID { get; set; }
            public int ProductId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<DateTime>>
        {
            private readonly IProductRepository _productRepo;
            private readonly IProductLineRepository _productLineRepo;
            private readonly IContractRepository _contractRepo;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IProductRepository productRepo, IProductLineRepository productLineRepo, IContractRepository contractRepo)
            {
                _mapper = mapper;
                _contractRepo = contractRepo;
                _productLineRepo = productLineRepo;
                _productRepo = productRepo;
            }

            public async Task<Result<DateTime>> Handle(Query request, CancellationToken cancellationToken)
            {                   
                var prodDuration = _productRepo.GetProductDuration(request.ProductId);
                var finishDate = DateTime.Now.AddDays(prodDuration);                
                return await Task.FromResult(Result<DateTime>.Success(finishDate));
            }
        }
    }
}
