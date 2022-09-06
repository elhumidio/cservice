using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TSalaryType")]
    public partial class SalaryType
    {
        public SalaryType()
        {
            Tsalaries = new HashSet<Salary>();
        }

        public int IdsalaryType { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public int? ChkActive { get; set; }
        public int? DisplayOrder { get; set; }

        public virtual TsturijobsLang Ids { get; set; } = null!;
        public virtual ICollection<Salary> Tsalaries { get; set; }
    }
}
