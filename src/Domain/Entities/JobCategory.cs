using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TJobCategory")]
    public partial class JobCategory
    {
        public int IdjobCategory { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
    }
}
