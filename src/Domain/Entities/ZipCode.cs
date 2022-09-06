using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TZipCode")]
    public partial class ZipCode
    {
        public int IdzipCode { get; set; }
        public int Idcountry { get; set; }
        public int Idregion { get; set; }
        public string? City { get; set; }
        public string Zip { get; set; } = null!;
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int? Idcity { get; set; }
    }
}
