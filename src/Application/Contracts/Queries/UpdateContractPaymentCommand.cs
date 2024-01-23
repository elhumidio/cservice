using Application.Core;
using MediatR;

namespace Application.Contracts.Queries
{
    public class UpdateContractPaymentCommand : IRequest<Result<bool>>
    {
        public string? SessionId { get; set; }
    }
}
