namespace TURI.ContractService.Contracts.Contract.Models.Response
{
    public class OfferInfoBuildUrlResponse
    {
        public int OfferId { get; set; }
        public string? Title { get; set; }
        public int RegionId { get; set; }
        public int CityId { get; set; }
        public int SiteId { get; set; }
    }
}
