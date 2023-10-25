namespace Domain.Entities
{
    public partial class ContractPayment
    {
        public int IdcontractPayment { get; set; }
        public int Idcontract { get; set; }
        public DateTime DataPayment { get; set; }
        public decimal Payment { get; set; }
        public decimal? PaymentWithoutTax { get; set; }
    }
}
