namespace Application.AuxiliaryData.DTO
{
    public partial class SalaryTypeDTO
    {
        public int IdsalaryType { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public int? ChkActive { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
