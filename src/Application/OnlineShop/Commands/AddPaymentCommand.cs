using Application.Core;
using MediatR;

namespace Application.OnlineShop.Commands
{
    public class AddPaymentCommand : IRequest<Result<bool>>
    {
        public int Idcontract { get; set; }
        public decimal Payment { get; set; }
        public decimal? PaymentWithoutTax { get; set; }
        public DateTime DataPayment { get; set; }
    }
}
