namespace Domain.Classes
{
    public class UserContractAvailableUnitsData
    {
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public int TypeUser { get; set; }
        public string? ContractConcept { get; set; }
        public DateTime? FinishDate { get; set; }
        public int JobVacAvailable { get; set; }
        public bool ChkPack { get; set; }
        public string? GrcontactId { get; set; }
    }
}
