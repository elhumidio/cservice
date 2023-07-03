using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class GenericOfferCounter
    {

        public List<CountByOffer> results { get; set; }
        public GenericOfferCounter()
        {
            results = new List<CountByOffer>();
        }
    }
    public class CountByOffer
    {

        [JsonProperty("jobOfferId")] 
        public int jobId { get; set; }

        [JsonProperty("applicants")]
        public int Applicants { get; set; }

        public CountByOffer()
        {
            jobId = 0;
            Applicants = 0;
        }
    }
}
