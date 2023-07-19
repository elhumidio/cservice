using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TCurrency")]
    public class Currency
    {
        public int IDCurrency { get; set; }
        public int IDSite { get; set; }
        public int IDSLanguage { get; set; }
        public string BaseName { get; set; }
    }
}
