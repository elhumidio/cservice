namespace Domain.DTO
{
    public class ContractsDistDto
    {
        public int ContractId { get; set; }
        public int ProductId { get; set; }
        public bool IsPack { get; set; }
        public string BaseName { get; set; }
        public int TotalAvailableFeatured { get; set; }
        public int TotalAvailablestandard { get; set; }
        public int TotalFeaturedUnits { get; set; }
        public int TotalStandardUnits { get; set; }
        public string FinishDate { get; set; }
        public bool IsRegionRestricted { get; set; }
        public bool? IsPayed { get; set; }
        public DateTime StartDate { get; set; }
        public int IdJobVacType { get; set; }
    }
}
