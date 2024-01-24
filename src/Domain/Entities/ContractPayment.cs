namespace Domain.Entities
{
    public partial class ContractPayment
    {
        public int IdcontractPayment { get; set; }
        public int Idcontract { get; set; }
        public DateTime DataPayment { get; set; }
        public decimal Payment { get; set; }
        public decimal? PaymentWithoutTax { get; set; }
        public decimal? CouponDiscount { get; set; }
        public decimal? TaxAmount { get; set; }
        public string? Currency { get; set; }
        public decimal ConvertRate { get; set; }
        public bool? Finished { get; set; }
    }
}
