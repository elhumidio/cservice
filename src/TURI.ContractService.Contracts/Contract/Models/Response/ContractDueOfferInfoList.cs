using TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder;

namespace TURI.ContractService.Contracts.Contract.Models.Response
{
    public class ContractDueOfferInfoList
    {
        public List<ContractCreationResponse> contractCreationResponses { get; set; } = new List<ContractCreationResponse>();
    }
}
