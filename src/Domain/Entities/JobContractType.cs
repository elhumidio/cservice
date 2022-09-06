using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TJobContractType")]
    public partial class JobContractType
    {
        public int IdjobContractType { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public int? DisplayOrder { get; set; }
    }
}
