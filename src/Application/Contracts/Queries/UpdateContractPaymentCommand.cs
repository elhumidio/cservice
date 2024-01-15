using Application.Core;
using MediatR;

namespace Application.Contracts.Queries
{
    public class UpdateContractPaymentCommand : IRequest<Result<bool>>
    {
        public int amount_discount { get; set; }
        public int amount_tax { get; set; }
        public int amount_subtotal { get; set; }
        public int amount_total { get; set; }
        public string? SessionId { get; set; }
    }
}
