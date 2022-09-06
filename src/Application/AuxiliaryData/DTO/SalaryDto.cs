namespace Application.AuxiliaryData.DTO
{
    public partial class SalaryDTO
    {
        public int Idsalary { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string Value { get; set; } = null!;
        public int IdsalaryType { get; set; }
        public int? Idcurrency { get; set; }
        public int? ChkActive { get; set; }
    }
}
