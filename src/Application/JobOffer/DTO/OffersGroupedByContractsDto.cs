using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JobOffer.DTO
{
    public class OffersGroupedByContractsDto
    {
        public int ContractId { get; set; }
        public List<JobOfferDto> ListOffers { get; set; }

        public OffersGroupedByContractsDto() {

            ListOffers = new List<JobOfferDto>();
        }
    }
}
