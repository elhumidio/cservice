namespace TURI.ContractService.Contracts.Contract.Models.Requests
{
    public class ListCompaniesIdsRequest
    {
        public List<int> CompaniesIds { get; set; }
        public int? State { get; set; }
        public DateTime? MaxDate { get; set; }
    }


}
