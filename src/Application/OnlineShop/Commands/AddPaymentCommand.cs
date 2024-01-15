using Application.Core;
using MediatR;

namespace Application.OnlineShop.Commands
{
    public class AddPaymentCommand : IRequest<Result<bool>>
    {
        public int amount_discount { get; set; }
        public int amount_tax { get; set; }
        public int amount_subtotal { get; set; }
        public int amount_total { get; set; }
        public string? SessionId { get; set; }

    }
}
