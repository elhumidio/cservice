using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class GetCompanyRequest
    {
        public int RegionId { get; set; }
        public int CountryId { get; set; }
        public string Email { get; set; }
        public int ContractId { get; set; }
        public int IdJobVacType { get; set; }
        public int CompanyId { get; set; }
        public int? IdEnterpriseUser { get; set; }
    }
}
