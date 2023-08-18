namespace TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder
{
    public class ContractProductResponse
    {
        public int Idcontract { get; set; }
        public int Idproduct { get; set; }
        public int? Idpromotion { get; set; }
        public int Units { get; set; }
        public decimal Price { get; set; }
        public bool ChkAllEnteprise { get; set; }
        public DateTime? ProductContractDate { get; set; }
        public string? IdsalesForce { get; set; }
        public decimal? CommercialDiscount { get; set; }
        public decimal? CouponDiscount { get; set; }
    }
}
