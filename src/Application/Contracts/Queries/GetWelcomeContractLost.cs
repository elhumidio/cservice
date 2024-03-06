using Application.ContractCRUD.Query;
using Application.Core;
using Domain.Repositories;
using MediatR;
using TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder;

namespace Application.Contracts.Queries
{
    public class GetWelcomeContractLost
    {
        public class Get : IRequest<Result<List<ContractCreationResponse>>>
        {
        }

        public class Handler : IRequestHandler<Get, Result<List<ContractCreationResponse>>>
        {
            private readonly IContractPaymentRepository _contractPayRepo;
            private readonly IMediator _mediatr;

            public Handler(IContractPaymentRepository contractPayRepo, IMediator mediator)
            {
                _contractPayRepo = contractPayRepo;
                _mediatr = mediator;
            }

            public async Task<Result<List<ContractCreationResponse>>> Handle(Get request, CancellationToken cancellationToken)
            {
                var contracts = _contractPayRepo.GetWelcomeContractsDueOfferNotPayed();

                var listResult = await Task.WhenAll(contracts.Select(async contract =>
                {
                    var contractInfo = await _mediatr.Send(new GetContractAndRelated.Query { ContractId = contract });
                    return contractInfo.Value;
                }));

                return Result<List<ContractCreationResponse>>.Success(listResult.ToList());
            }

        }
    }
}
