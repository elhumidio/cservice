using Application.Core;
using Domain.Repositories;
using MediatR;

namespace Application.Contracts.Queries
{
    public class DeleteContractAndRelated
    {
        public class Query : IRequest<Result<bool>>
        {
            public int ContractID { get; set; }
       
        }

        public class Handler : IRequestHandler<Query, Result<bool>>
        {
            private readonly IContractRepository _contract;
            public Handler(IContractRepository contractRepository)
            {
                _contract = contractRepository;
            }

            public async Task<Result<bool>> Handle(Query request, CancellationToken cancellationToken)
            {
                var ret = _contract.DeleteContractInfo(request.ContractID);
                    
                return Result<bool>.Success(ret);
            }
        }
    }
}
