using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TEnterpriseUser")]
    public partial class EnterpriseUser
    {
        public int IdenterpriseUser { get; set; }
        public int Idsuser { get; set; }
        public int Identerprise { get; set; }
        public int Idperiod { get; set; }
        public string ContactName { get; set; } = null!;
        public string ContactPosition { get; set; } = null!;
        public string? ContactPhone { get; set; }
        public int? MaxJobVacancies { get; set; }
        public bool? ChkMarketing { get; set; }
        public string ContactSurname { get; set; } = null!;
        public bool? ChkNotifications { get; set; }
        public bool? ChkPromos { get; set; }
        public string? SalesforceId { get; set; }
        public DateTime? Sftimestamp { get; set; }
        public string? PrefixContactPhone { get; set; }
    }
}
