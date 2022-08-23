using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TSite")]
    public partial class Site
    {
        public int Idsite { get; set; }
        public string? BaseName { get; set; } = null!;
    }
}
