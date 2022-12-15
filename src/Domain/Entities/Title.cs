using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Titles")]
    public partial class Title
    {
        public int Id { get; set; }
        public string Isco08 { get; set; } = null!;
        public string Isco88 { get; set; } = null!;
        public string? Description { get; set; }
    }
}
