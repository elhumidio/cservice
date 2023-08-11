namespace Domain.DTO.Requests
{
    public class UpdateContractProductSForceId
    {
        public int ContractId { get; set; }
        public string? ContractSalesforceId { get; set; }

        public List<ContractProdSForceId> ContractProductSalesforceIds { get; set; } = new List<ContractProdSForceId>();
    }
}
