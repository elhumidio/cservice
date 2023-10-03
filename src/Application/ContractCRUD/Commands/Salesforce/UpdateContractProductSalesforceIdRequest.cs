using Application.ContractCreation.Dto;
using Application.Core;
using MediatR;
using System.Runtime.Serialization;

namespace Application.ContractCRUD.Commands.Salesforce
{
    [DataContract]
    public class UpdateContractProductSalesforceIdRequest : IRequest<Result<bool>>
    {
        [DataMember]
        public int ContractId { get; set; }

        [DataMember]
        public string ContractSalesForceId { get; set; }

        [DataMember]
        public List<ContractProductSalesforceId> ContractProductSalesforceIds { get; set; } = new List<ContractProductSalesforceId>();
    }
}
