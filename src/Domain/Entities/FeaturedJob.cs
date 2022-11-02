using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TFeaturedJob")]
    public partial class FeaturedJob
    {
        public int Id { get; set; }
        public int Idsite { get; set; }
        public string? Category { get; set; }
        public string? Title { get; set; }
        public string? CompanyName { get; set; }
        public string? Location { get; set; }
        public string? Url { get; set; }
        public string? CategoryMoreJobs { get; set; }
        public string? CategoryMoreJobsUrl { get; set; }
        public string? FeaturedAdImage { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
