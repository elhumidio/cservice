using TURI.ContractService.Contracts.Contract.Models.Partials;

namespace TURI.ContractService.Contracts.Contract.Models.Requests
{
    public class WrapperContractProductSalesforceIdRequest
    {
        public int ContractId { get; set; }

        public string ContractSalesForceId { get; set; }

        public List<ContractProductSalesforceIdRequest> ContractProductSalesforceIds { get; set; } = new List<ContractProductSalesforceIdRequest>();
    }
}
