using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JobOffer.DTO
{
    public class FlowOfferDTO
    {
        public string Title { get; set; }
        public string Location { get; set; }
        public string CompanyName { get; set; }
        public string ExternalURL { get; set; }
        public int AreaId { get; set; }
        public int CountryId { get; set; }
        public int RegionId { get; set; }
        public int SiteId { get; set; }
    }
}
