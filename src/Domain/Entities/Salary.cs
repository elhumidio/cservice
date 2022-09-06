using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TSalary")]
    public partial class Salary
    {
        public int Idsalary { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string Value { get; set; } = null!;
        public int IdsalaryType { get; set; }
        public int? Idcurrency { get; set; }
        public int? ChkActive { get; set; }

        public virtual SalaryType Ids { get; set; } = null!;
    }
}
