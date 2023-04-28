using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class OfferMinInfoAtsDto
    {
        public string Title { get; set; }
        public string? PublishedDate { get; set; }
        public string FiledDate { get; set; }
        public string? FinishDate { get; set; }
        public string Status { get; set; }
        public string ExternalId { get; set; }
        public int TurijobsId { get; set; }

    }
}
