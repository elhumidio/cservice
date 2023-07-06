namespace Domain.Entities
{
    public partial class FeedsAggregatorsLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? FeedName { get; set; }
        public int AreaId { get; set; }
        public int RegionId { get; set; }
        public int TotalOffers { get; set; }
        public string? AreaName { get; set; }
        public string? RegionName { get; set; }
    }
}
