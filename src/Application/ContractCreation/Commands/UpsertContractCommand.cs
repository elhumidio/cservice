using Application.ContractCreation.Dto;
using Application.Core;
using MediatR;
using System.Runtime.Serialization;

namespace Application.ContractCreation.Commands
{
    [DataContract]
    public class UpsertContractCommand : IRequest<Result<ContractCreationDomainResponse>>
    {
        [DataMember]
        public int IDSUser { get; set; } = -1;

        [DataMember]
        public int IDEnterprise { get; set; } = -1;

        [DataMember]
        public int IDEnterpriseUSer { get; set; } = -1;

        [DataMember]
        public int IDRegion { get; set; } = -1;

        [DataMember]
        public int IDSite { get; set; } = -1;

        [DataMember]
        public int IDSLanguage { get; set; } = -1;

        [DataMember]
        public string? SalesforceAccountId { get; set; }

        [DataMember]
        public List<int>? ProductsList { get; set; }

        [DataMember]
        public string? Concept { get; set; }

        [DataMember]
        public int? PaymentMethod { get; set; }




    }



}
