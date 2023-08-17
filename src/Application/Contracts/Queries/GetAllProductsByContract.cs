using Application.Core;
using AutoMapper;
using Domain.DTO;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class GetAllProductsByContract
    {
        public class GetProducts : IRequest<Result<List<ContractProductShortDto>>>
        {
            public int ContractId { get; set; }
            public int LanguageID { get; set; }
            public int SiteId { get; set; }
        }

        public class Handler : IRequestHandler<GetProducts, Result<List<ContractProductShortDto>>>
        {
            private readonly IContractRepository _contractRepo;

            public Handler(IMapper mapper, IContractRepository contractRepo)
            {
                _contractRepo = contractRepo;
            }

            public async Task<Result<List<ContractProductShortDto>>> Handle(GetProducts request, CancellationToken cancellationToken)
            {
                var products = await _contractRepo.GetAllProductsByContract(request.ContractId, request.LanguageID, request.SiteId);
                return Result<List<ContractProductShortDto>>.Success(products);
            }
        }
    }
}
