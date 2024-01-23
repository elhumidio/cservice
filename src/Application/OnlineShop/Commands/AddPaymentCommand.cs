using Application.Core;
using MediatR;

namespace Application.OnlineShop.Commands
{
    public class AddPaymentCommand : IRequest<Result<bool>>
    {
        public decimal amount_discount { get; set; }
        public decimal amount_tax { get; set; }
        public decimal amount_subtotal { get; set; }
        public decimal amount_total { get; set; }
        public string? SessionId { get; set; }
        public string? Currency { get; set; }

    }
}
