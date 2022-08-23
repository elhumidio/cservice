using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AuxiliaryData.DTO
{
    public partial class SiteDTO
    {  
        public int Idsite { get; set; }
        public string? BaseName { get; set; } = null!;

    }
}
