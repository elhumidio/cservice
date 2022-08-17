using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("EquestIndustry")]
    public partial class EquestIndustry
    {
        public short IdindustryCode { get; set; }
        public string? Name { get; set; }
        public short? EquivalentId { get; set; }
    }
}
