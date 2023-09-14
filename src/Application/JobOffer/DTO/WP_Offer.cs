using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JobOffer.DTO
{
    public class WP_Offer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string EnterpriseName { get; set; }
        public string Location { get; set; }
        public List<string> Tags { get; set; }
        public string DateDiff { get; set; }
        public string ImageURL { get; set; }
        public string URL { get; set; }
    }
}
