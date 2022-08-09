using System.ComponentModel.DataAnnotations.Schema;

namespace API.DataContext
{
    [Table("TRegJobVacMatching")]
    public partial class RegJobVacMatching
    {
        public int IdjobMatching { get; set; }
        public int IdjobVacancy { get; set; }
        public string? ExternalId { get; set; }
        public int Identerprise { get; set; }
        public string? Redirection { get; set; }
        public string? AppEmail { get; set; }
        public string? Idintegration { get; set; }
    }
}
