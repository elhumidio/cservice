using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TResidenceType")]
    public partial class ResidenceType
    {
        public int IdresidenceType { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
    }
}
