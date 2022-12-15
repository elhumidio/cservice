using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JobOffer.DTO
{
    public class ContractOwnerRequestDto
    {
        public List<ContractOwnerDto> ContractOwnerDtos { get; set; }
        

    }

    public class ContractOwnerDto{
        public int ContractId { get; set; }
        public int OwnerId { get; set; }    

    }
}
