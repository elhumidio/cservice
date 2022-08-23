using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AuxiliaryData.DTO
{
    public partial class CountryDTO
    {
        public int Idcountry { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public string? PhoneIntPref { get; set; }
        public string Nationality { get; set; } = null!;
        public string? FormatCandidateDoc1 { get; set; }
        public string? FormatZipCode { get; set; }
        public int? DisplayOrder { get; set; }

    }
}
