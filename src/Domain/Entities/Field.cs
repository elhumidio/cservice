using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TField")]
    public class Field
    {
        public int IDField { get; set; }
        public int IDSite { get; set; }
        public int IDSLanguage { get; set; }
        public string BaseName { get; set; }
        public int IDMacroSector { get; set; }
    }
}
