using Application.Contracts.DTO;

namespace Application.EnterpriseContract.DTO
{
    public class RegEnterpriseContractDTO
    {
        public int Identerprise { get; set; }
        public int Idcontract { get; set; }
        public int IdjobVacType { get; set; }
        public int Units { get; set; }
        public int UnitsUsed { get; set; }
        public int? IdjobVacTypeComp { get; set; }

        public virtual ContractDTO IdcontractNavigation { get; set; } = null!;
    }
}
