using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class ZoneUrl
    {
        public string Url { get; set; } = null!;
        public int? Idcountry { get; set; }
        public int? Idregion { get; set; }
        public int? Idcity { get; set; }
    }
}
