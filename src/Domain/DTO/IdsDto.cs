using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class IdsDto
    {
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public int CountryId { get; set; }
        public int ZipCodeId { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }

    }
}
