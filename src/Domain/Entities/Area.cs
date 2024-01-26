using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TArea")]
    public partial class Area
    {
        public Area()
        {
            JobTitlesAreas = new HashSet<JobTitleArea>();
            TsubAreas = new HashSet<TsubArea>();
        }

        public int Idarea { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public string? Subdomain { get; set; }
        public int? ChkActive { get; set; }
        public int? NumVacancies { get; set; }
        public int? Threshold { get; set; }
        public int? IscoDefault { get; set; }
        public int? FkTranslationId { get; set; }

        public virtual ICollection<JobTitleArea> JobTitlesAreas { get; set; }
        public virtual ICollection<TsubArea> TsubAreas { get; set; }
    }
}
