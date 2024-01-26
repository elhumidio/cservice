
namespace Domain.Entities
{
    public class JobTitlesRelationship
    {
        public int FkJobTitleId { get; set; }
        public int FkJobTitleRelatedId { get; set; }
        public int? Priority { get; set; }
    }
}
