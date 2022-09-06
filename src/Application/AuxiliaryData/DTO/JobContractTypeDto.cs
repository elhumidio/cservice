namespace Application.AuxiliaryData.DTO
{
    public partial class JobContractTypeDTO
    {
        public int IdjobContractType { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public int? DisplayOrder { get; set; }
    }
}
