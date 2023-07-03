using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JobOffer.Commands
{
    public class GetFlowOfferByIdCommand
    {
        public int OfferId { get; set; }
        public int LanguageId { get; set; }
    }
}
