using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TCity")]
    public partial class City
    {
        public int Idcity { get; set; }
        public int? Idcountry { get; set; }
        public int? Idregion { get; set; }
        public string? Name { get; set; }
        public string? Cpro { get; set; }
        public string? Cmun { get; set; }
        public string? Dc { get; set; }
    }
}
