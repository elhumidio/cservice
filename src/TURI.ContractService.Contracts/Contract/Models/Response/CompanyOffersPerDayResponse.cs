namespace TURI.ContractService.Contracts.Contract.Models.Response
{
    public class CompanyOffersPerDayResponse
    {
        public DateTime Date { get; set; }
        public int EnterpriseId { get; set; }
        public int OffersPublished { get; set; }
    }
}
