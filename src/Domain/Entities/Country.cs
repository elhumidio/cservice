using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Tcountry")]
    public partial class Country
    {
        public int Idcountry { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public string? PhoneIntPref { get; set; }
        public string Nationality { get; set; } = null!;
        public string? FormatCandidateDoc1 { get; set; }
        public string? FormatZipCode { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
