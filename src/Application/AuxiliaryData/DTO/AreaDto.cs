namespace Application.AuxiliaryData.DTO
{
    public partial class AreaDTO
    {
        public int Idarea { get; set; }
        public int Idsite { get; set; }
        public int Idslanguage { get; set; }
        public string BaseName { get; set; } = null!;
        public string? Subdomain { get; set; }
        public int? ChkActive { get; set; }
        public int? NumVacancies { get; set; }
        public int? Threshold { get; set; }
    }
}
