using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TstypeUser")]
    public partial class TypeUser
    {
        public int IdstypeUser { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string Idbduser { get; set; } = null!;
        public string BaseName { get; set; } = null!;
    }
}
