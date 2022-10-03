using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Tlogo")]
    public partial class Logo
    {
        public int Idlogo { get; set; }
        public int Identerprise { get; set; }
        public int? Idbrand { get; set; }
        public string UrlImgBig { get; set; } = null!;
        public string? UrlImgSmall { get; set; }
        public int? ShowOrder { get; set; }
        public DateTime CreationDate { get; set; }
        public int? OldIdlogo { get; set; }
    }
}
