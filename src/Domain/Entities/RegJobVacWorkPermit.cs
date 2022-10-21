using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("TRegJobVacWorkPermit")]
    public partial class RegJobVacWorkPermit
    {
        public int IdjobVacancy { get; set; }
        public int IdworkPermit { get; set; }
    }
}
