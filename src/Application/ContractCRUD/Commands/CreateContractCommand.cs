using Application.ContractCreation.Dto;
using Application.Core;
using Domain.DTO.Products;
using MediatR;
using System.Runtime.Serialization;

namespace Application.ContractCRUD.Commands
{
    [DataContract]
    public class CreateContractCommand : IRequest<Result<ContractCreationDomainResponse>>
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
        public List<ProductUnits>? ProductsList { get; set; }

        [DataMember]
        public string? Concept { get; set; }

        [DataMember]
        public int? PaymentMethod { get; set; }

        [DataMember]
        public int CountryId { get; set; } // to get prices by country

    }
}
