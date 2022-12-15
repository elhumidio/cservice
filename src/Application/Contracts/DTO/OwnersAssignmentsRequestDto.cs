using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTO
{
    public class OwnersAssignmentsRequestDto
    {
        public List<int> ContractsList { get; set; }
        public List<int> OwnersList { get; set; }

        public OwnersAssignmentsRequestDto()
        {
            ContractsList = new List<int>();
            OwnersList = new List<int>();
        }
    }
}
