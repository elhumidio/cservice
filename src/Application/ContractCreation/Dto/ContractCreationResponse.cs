using Domain.Entities;

namespace Application.ContractCreation.Dto
{
    public class ContractCreationResponse
    {
        public Contract Contract { get; set; } = new Contract();
        public List<ContractProduct> ContractProducts { get; set; } = new List<ContractProduct> { };
        public List<RegEnterpriseContract> RegEnterpriseContracts { get; set; } = new List<RegEnterpriseContract> { };
        public List<RegEnterpriseConsum> RegEnterpriseConsums { get; set; } = new List<RegEnterpriseConsum> { };
        public List<ProductLine> ProductLines { get; set; } = new List<ProductLine> { };
    }
}
