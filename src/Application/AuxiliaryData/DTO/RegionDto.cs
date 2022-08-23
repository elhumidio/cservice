using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AuxiliaryData.DTO
{
    public partial class RegionDTO
    {
        public int Idregion { get; set; }
        public int Idcountry { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public string? Ccaa { get; set; }
        public int? ChkActive { get; set; }
        public int? NumVacancies { get; set; }
        public int? Threshold { get; set; }

    }
}
