using System.Runtime.Serialization;

namespace TURI.ContractService.Contracts.Contract.Models.Requests
{
    public class UpdateSalesforceIdsRequest
    {
        [DataMember]
        public int ContractId { get; set; }

        [DataMember]
        public string ContractSalesForceId { get; set; }

        [DataMember]
        public List<ContractProductSFInfo> ContractProductSalesforceIds { get; set; } = new List<ContractProductSFInfo>();
    }
}
