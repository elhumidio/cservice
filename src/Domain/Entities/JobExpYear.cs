using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TJobExpYear")]
    public partial class JobExpYear
    {
        public int IdjobExpYears { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public string? BaseNameShort { get; set; }
        public int? ShowOrder { get; set; }

        public virtual TsturijobsLang Ids { get; set; } = null!;
    }
}
