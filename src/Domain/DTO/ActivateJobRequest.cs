using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ActivateJobRequest
    {
        public int OwnerId { get; set; }
        public int JobId { get; set; }
    }
}
