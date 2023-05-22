namespace Domain.DTO
{
    public class ProductsPurchasedDto
    {
        public int IDContract { get; set; }
        public string BaseName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int? SiteID { get; set; }
        public decimal Price { get; set; }
    }
}
