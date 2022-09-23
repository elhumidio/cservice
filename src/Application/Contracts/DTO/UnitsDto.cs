using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTO
{
    public class UnitsDto
    {
        public int JobVacTypeId { get; set; }
        public int TotalUnits { get; set; }
        public int IDContract { get; set; }
        public int AvailableUnits { get; set; }
        public string JobVacTypeDesc { get; set; }
    }
}
