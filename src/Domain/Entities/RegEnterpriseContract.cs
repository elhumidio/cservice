using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TregEnterpriseContract")]
    public partial class RegEnterpriseContract
    {
        public int Identerprise { get; set; }
        public int Idcontract { get; set; }
        public int IdjobVacType { get; set; }
        public int Units { get; set; }
        public int UnitsUsed { get; set; }
        public int? IdjobVacTypeComp { get; set; }

        public virtual Contract IdcontractNavigation { get; set; } = null!;
    }
}
