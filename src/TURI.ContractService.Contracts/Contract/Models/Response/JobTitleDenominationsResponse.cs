namespace TURI.ContractService.Contracts.Contract.Models.Response
{
    public class JobTitleDenominationsResponse
    {
        public int Id { get; set; }
        public string Denomination { get; set; } = null!;
        public int FkJobTitle { get; set; }
    }
}
