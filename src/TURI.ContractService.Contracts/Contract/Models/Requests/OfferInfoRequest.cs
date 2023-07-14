namespace TURI.ContractService.Contracts.Contract.Models.Requests
{
    public class OfferInfoRequest
    {
        public int[] OfferIds { get; set; }
        public int Site { get; set; }
        public int Language { get; set; }
    }
}
