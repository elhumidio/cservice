using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ListOffersRequest
    {
        public List<int> Offers { get; set; }
        public ListOffersRequest()
        {
            Offers = new List<int>();

        }
    }
}
