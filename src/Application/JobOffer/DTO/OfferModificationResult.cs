using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.JobOffer.DTO
{
    public class OfferModificationResult
    {
        public bool IsSuccess { get; set; }
        public OfferDto Value { get; set; }
        public List<string> Failures { get; set; }

        public static OfferModificationResult Success(OfferDto value) => new OfferModificationResult { IsSuccess = true, Value = value };
        public static OfferModificationResult Failure(List<string> failures) => new OfferModificationResult { IsSuccess = false, Failures = failures };
    }
}
