using Application.Contracts.DTO;
using Application.JobOffer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class ContractResult
    {
        public bool IsSuccess { get; set; }
        public ContractDto Value { get; set; }
        public List<string> Failures { get; set; }

        public static ContractResult Success(ContractDto value) => new ContractResult { IsSuccess = true, Value = value };

        public static ContractResult Failure(List<string> failures) => new ContractResult { IsSuccess = false, Failures = failures };
    }
}
