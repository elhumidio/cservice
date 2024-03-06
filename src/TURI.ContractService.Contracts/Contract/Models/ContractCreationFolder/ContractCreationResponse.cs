namespace TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder
{
    public class ContractCreationResponse
    {
        public ContractResponse Contract { get; set; } = new ContractResponse();
        public List<ContractProductResponse> ContractProducts { get; set; } = new List<ContractProductResponse> { };
        public List<RegEnterpriseContractResponse> RegEnterpriseContracts { get; set; } = new List<RegEnterpriseContractResponse> { };
        public List<RegEnterpriseConsumResponse> RegEnterpriseConsums { get; set; } = new List<RegEnterpriseConsumResponse> { };
        public List<ProductLineResponse> ProductLines { get; set; } = new List<ProductLineResponse> { };
        public List<ContractProductShortDtoResponse> contractProductShortDtoResponses { get; set; } = new List<ContractProductShortDtoResponse> { };
        public string Currency { get; set; }
        public int CountryId { get; set; }
        public string OpportunityState { get; set; }
    }
}
