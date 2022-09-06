using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TDegree")]
    public partial class Degree
    {
        public int Iddegree { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public string? ShortName { get; set; }
        public byte? Ordre { get; set; }
        public int? Weight { get; set; }
    }
}
