using System.ComponentModel.DataAnnotations.Schema;

namespace API.DataContext
{
    [Table("TCountryIso")]
    public partial class CountryIso
    {
        public int IdcountryIso { get; set; }
        public string? Iso { get; set; }
        public string? Name { get; set; }
        public bool? Cee { get; set; }
        public int? Idcountry { get; set; }
    }
}
