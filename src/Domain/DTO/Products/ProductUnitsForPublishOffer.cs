namespace Domain.DTO.Products
{
    public class ProductUnitsForPublishOffer
    {
        public int ProductId { get; set; }
        public int ContractIdFinishingSoonest { get; set; }
        public string? ProductName { get; set; }
        public int Credits { get; set; }
        public int IdJobVacType { get; set; }
        public bool IsBlindable { get; set; }
        public bool IsStandard { get; set; }
        public bool IsFeatured { get; set; }
        public bool InternationalDifusion { get; set; }
        public bool NationalDifusion { get; set; }
    }
}
