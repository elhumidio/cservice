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
                // la mayor de las tres
                var contract = _contractRepo.Get(request.ContractID).First();
                var contractFinishDate = contract != null ? contract.FinishDate : DateTime.Now;
                var prodDuration = _productRepo.GetProductDuration(request.ProductId);
                var prodBasedFinishDate = DateTime.Now.AddDays(prodDuration);
                var prodLineDuration = _productLineRepo.GetProductLineDuration(request.ProductId);
                var prodLineBasedFinishDate = DateTime.Now.AddDays(prodLineDuration);
                DateTime firstCompareWinner;
                DateTime secondCompareWinner;
                if (contractFinishDate.Value.Date.CompareTo(prodBasedFinishDate.Date) < 1)
                {
                    firstCompareWinner = contractFinishDate.Value.Date;
                }
                else
                {
                    firstCompareWinner = prodBasedFinishDate.Date;
                }
                if (firstCompareWinner.Date.CompareTo(prodLineBasedFinishDate) < 1)
                {
                    secondCompareWinner = firstCompareWinner;
                }
                else
                {
                    secondCompareWinner = prodLineBasedFinishDate.Date;
                }
                return await Task.FromResult(Result<DateTime>.Success(secondCompareWinner));

            }
        }

    }
}
