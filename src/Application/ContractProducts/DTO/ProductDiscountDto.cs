using System.Runtime.Serialization;

namespace Application.ContractProducts.DTO
{
    [DataContract]
    public class ProductDiscountDto
    {
        [DataMember]
        public int ProductId { get; set; }
        [DataMember]
        public decimal CommercialDiscount { get; set; }
        [DataMember]
        public int ContractId { get; set; }
    }
}
