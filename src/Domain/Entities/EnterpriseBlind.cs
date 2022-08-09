using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TEnterpriseBlind")]
    public partial class EnterpriseBlind
    {
        public int Identerprise { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public int Idcountry { get; set; }
        public int Idregion { get; set; }
        public int Idfield { get; set; }
        public string City { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Employees { get; set; }
        public string? Web { get; set; }
        public int? IdsubField { get; set; }
        public int? Idcity { get; set; }
    }
}
