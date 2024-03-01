namespace Domain.DTO
{
    public class ContractPaymentDto
    {
        public int? IdcontractPayment { get; set; } = 0;
        public int? Idcontract { get; set; } = 0;
        public DateTime DataPayment { get; set; }
        public decimal Payment { get; set; }
        public decimal? PaymentWithoutTax { get; set; } = 0;
        public decimal? CouponDiscount { get; set; } = 0;
        public decimal? TaxAmount { get; set; } = 0;
        public string? Currency { get; set; }
        public decimal? ConvertRate { get; set; } = 0;
        public bool? Finished { get; set; }
    }
}
