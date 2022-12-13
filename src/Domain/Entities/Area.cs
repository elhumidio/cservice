using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TArea")]
    public partial class Area
    {
        public Area()
        {
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

        public int IscoDefault { get; set; }

        public virtual TsturijobsLang Ids { get; set; } = null!;
        public virtual ICollection<TsubArea> TsubAreas { get; set; }
    }
}
