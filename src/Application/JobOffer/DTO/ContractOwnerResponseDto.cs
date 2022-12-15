using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JobOffer.DTO
{
    public class ContractOwnerResponseDto
    {
        public int ContractId { get; set; }
        public int OwnerId{ get; set; }
        public List<JobOfferDto> Offers { get; set; }
        public ContractOwnerResponseDto() {

            Offers = new List<JobOfferDto>();
        }
    }
}
