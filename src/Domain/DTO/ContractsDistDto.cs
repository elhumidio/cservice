using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ContractsDistDto
    {
        public int ContractId { get; set; }
        public int ProductId { get; set; }
        public bool IsPack { get; set; }
        public string BaseName { get; set; }
        public int TotalAvailableFeatured { get; set; }
        public int TotalAvailablestandard { get; set; }
        public int TotalFeaturedUnits { get; set; }
        public int TotalStandardUnits { get; set; }
    }
}
