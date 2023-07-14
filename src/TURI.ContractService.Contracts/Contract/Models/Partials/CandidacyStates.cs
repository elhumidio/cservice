namespace TURI.ContractService.Contracts.Contract.Models.Partials
{
    public partial class CandidacyState
    {
        public int RegistrationId { get; set; }
        public int JobVacancyId { get; set; }
        public int Cvid { get; set; }
        public byte State { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
