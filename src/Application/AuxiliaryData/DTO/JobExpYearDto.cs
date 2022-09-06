namespace Application.AuxiliaryData.DTO
{
    public partial class JobExpYearDTO
    {
        public int IdjobExpYears { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public string? BaseNameShort { get; set; }
        public int? ShowOrder { get; set; }
    }
}
