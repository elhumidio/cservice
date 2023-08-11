namespace TURI.ContractService.Contracts.Contract.Models.ContractCreationFolder
{
    public class ProductLineResponse
    {
        public int IdproductLine { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public int Idproduct { get; set; }
        public int? IdjobVacType { get; set; }
        public int? IdserviceType { get; set; }
        public int Duration { get; set; }
        public short Units { get; set; }
        public int? IdgroupType { get; set; }
        public bool ChkConsumable { get; set; }
        public bool? ChkMain { get; set; }
    }
}
