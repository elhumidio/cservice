using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("EquestDegreeEquivalent")]
    public partial class EquestDegreeEquivalent
    {
        public int IdequestDegree { get; set; }
        public int Iddegree { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
    }
}
