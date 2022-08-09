using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TJobVacType")]
    public partial class JobVacType
    {
        public int IdjobVacType { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public int DaysOutstanding { get; set; }

        public virtual TsturijobsLang Ids { get; set; } = null!;
    }
}
