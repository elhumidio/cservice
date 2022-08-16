using Application.Contracts.DTO;

namespace Application.ContractProducts.DTO
{
    public class ContractProductDto
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

        public virtual ContractDto IdcontractNavigation { get; set; } = null!;
    }
}
