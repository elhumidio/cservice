using TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder;

namespace TURI.ContractService.Contracts.Contract.Models.Requests
{
    public class ContractCreateRequest
    {
        public int IDSUser { get; set; } = -1;

        public int IDEnterprise { get; set; } = -1;

        public int IDEnterpriseUSer { get; set; } = -1;

        public int IDRegion { get; set; } = -1;

        public int IDSite { get; set; } = -1;

        public int IDSLanguage { get; set; } = -1;

        public string? SalesforceAccountId { get; set; }

        public List<ProductUnitsRequest>? ProductsList { get; set; }

        public string? Concept { get; set; }

        public int? PaymentMethod { get; set; }

        public int CountryId { get; set; }
        public string? CheckoutSessionId { get; set; }

    }
}
