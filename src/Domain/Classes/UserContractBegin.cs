namespace Domain.Classes
{
    public class UserContractBegin
    {
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public int TypeUser { get; set; }
        public string? ContractConcept { get; set; }
        public DateTime? FinishDate { get; set; }
        public string? GrcontactId { get; set; }
    }
}
