using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("EquestCountryState")]
    public partial class EquestCountryState
    {
        public string IdcountryState { get; set; } = null!;
        public string? Name { get; set; }
        public short? EquivalentId { get; set; }
    }
}
