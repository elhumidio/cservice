namespace Application.AuxiliaryData.DTO
{
    public partial class BrandDTO
    {
        public int Idbrand { get; set; }
        public int Identerprise { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public int? OldIdbrand { get; set; }
        public bool? Active { get; set; }
    }
}
